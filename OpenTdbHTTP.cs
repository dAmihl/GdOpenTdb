using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

public class OpenTdbHTTP : HTTPRequest
{
    public const String OpenTDB_BASE_URL = "https://opentdb.com/";

    [Export] public EEncoding apiEncoding;
    [Export] public EType apiType;
    [Export] public EDifficulty apiDifficulty;

    [Signal]
    public delegate void QuestionsLoaded(Array<Question> questions);
    [Signal]
    public delegate void QuestionsLoadError(String errorMessage);

    public enum EEncoding
    {
        Default,
        UrlLegacy,
        Url3986,
        Base64
    }

    public enum EDifficulty
    {
        All,
        Easy,
        Medium,
        Hard
    }

    public enum EType
    {
        All,
        TrueFalse,
        MultipleChoice
    }

    public static class Encoding
    {
        public static String Default { get { return null; } }
        public static String UrlLegacy { get { return "urlLegacy"; } }
        public static String Url3986 { get { return "url3986"; } }
        public static String Base64 { get { return "base64"; } }

        public static String GetFromEnum(EEncoding e)
        {
            switch (e)
            {
                case EEncoding.UrlLegacy:
                    return UrlLegacy;
                case EEncoding.Url3986:
                    return Url3986;
                case EEncoding.Base64:
                    return Base64;
                case EEncoding.Default:
                    return Default;
                default:
                    return null;
            }
        }
    }

    public static class Difficulty
    {
        public static String Easy { get { return "easy"; } }
        public static String Medium { get { return "medium"; } }
        public static String Hard { get { return "hard"; } }

        public static String GetFromEnum(EDifficulty d)
        {
            switch (d)
            {
                case EDifficulty.Medium: 
                    return Medium;
                case EDifficulty.Hard: 
                    return Hard;
                case EDifficulty.Easy:
                    return Easy;
                case EDifficulty.All:
                    return null;
                default:
                    return null;
            }
        }
    }

    public static class Type
    {
        public static String MultipleChoice { get { return "multiple";  } }
        public static String TrueFalse { get { return "boolean";  } }

        public static String GetFromEnum(EType t)
        {
            switch (t)
            {
                case EType.MultipleChoice: 
                    return MultipleChoice;
                case EType.TrueFalse:
                    return TrueFalse;
                case EType.All:
                    return null;
                default:
                    return null;
            }
        }
    }


    public override void _Ready()
    {
        this.Connect("request_completed", this, "OnTriviaRequestComplete");
    }

    public void LoadTriviaQuestions(int amount)
    {
        FetchTriviaQuestions(amount, null, Difficulty.GetFromEnum(apiDifficulty), Type.GetFromEnum(apiType), Encoding.GetFromEnum(apiEncoding) );
    }

    private void FetchTriviaQuestions(int amount, String category = null, String difficulty = null, String type = null, String encoding = null)
    {
        string[] customHeaders = null;
        var validateSsl = true;

        String baseUrl = OpenTDB_BASE_URL + "api.php";
        String url = baseUrl + "?amount=" + amount;
        if (difficulty != null)
        {
            url += "&difficulty=" + difficulty;
        }
        if (category != null)
        {
            url += "&category=" + category;
        }
        if (type != null)
        {
            url += "&type=" + type;
        }
        if (encoding != null)
        {
            url += "&encoding=" + encoding;
        }
        GD.Print("[GdOpenTdb] Fetching from URL: " + url);
        var error = this.Request(url, customHeaders, validateSsl, HTTPClient.Method.Get);
        if (error != Error.Ok)
        {
            OnTriviaRequestError(error);
        }
    }

    private void OnTriviaRequestComplete(Result result, int response_code, string[] headers, byte[] body)
    {
        GD.Print("[GdOpenTdb] Loading Questions success!");
        var jsonStr = System.Text.Encoding.UTF8.GetString(body);
        JSONParseResult dict = JSON.Parse(jsonStr);
        
        if (dict.Error != 0)
        {
            GD.Print("Error: "+dict.Error+"/", dict.ErrorLine);
        }
        else
        {
            Dictionary parsed = dict.Result as Dictionary;
            Godot.Collections.Array results = parsed["results"] as Godot.Collections.Array;

            Array<Question> ParsedQuestions = new Array<Question>();

            foreach (Dictionary r in results)
            {
                ParsedQuestions.Add(CreateFromJsonResult(r));
            }

            EmitSignal(nameof(QuestionsLoaded), ParsedQuestions);
        }
    }

    // {category:Geography, correct_answer:False, difficulty:easy, incorrect_answers:[True], question:Greenland is covered with grass and Iceland covered with ice., type:boolean}
    // {category:Entertainment: Video Games, correct_answer:The Hotshot, difficulty:medium, incorrect_answers:[The Discard, The Elephant, The Mohawk], question:In WarioWare: Smooth Moves, which one of these is NOT a Form?, type:multiple}

    private Question CreateFromJsonResult(Dictionary res)
    {
        Question newQ = new Question();
        newQ.category = res["category"] as String;
        newQ.questionString = res["question"] as String;
        newQ.typeString = res["type"] as String;
        newQ.difficultyString = res["difficulty"] as String;
        newQ.questionDifficulty = Question.GetDifficultyFromString(res["difficulty"] as String);
        newQ.questionType = Question.GetTypeFromString(res["type"] as String);
        Array wrongAnswers = res["incorrect_answers"] as Array;
        Array<String> waS = new Array<string>();
        foreach (var wa in wrongAnswers)
        {
            waS.Add(wa as String);
        }
        newQ.wrongAnswers = waS;
        newQ.correctAnswer = res["correct_answer"] as String;
        return newQ;
    }

    private void OnTriviaRequestError(Error e)
    {
        String ErrorMsg = "[GdOpenTdb] Error loading Questions from OpenTDB. Error: " + e;
        GD.PrintErr(ErrorMsg);
        EmitSignal(nameof(QuestionsLoadError), ErrorMsg);
    }
}
