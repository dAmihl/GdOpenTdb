using Godot;
using System;
using Godot.Collections;

public class OpenTdbHTTP : HTTPRequest
{

    public const String OpenTDB_BASE_URL = "https://opentdb.com/";


    public override void _Ready()
    {
        this.Connect("request_completed", this, "OnTriviaRequestComplete");
    }

    public void LoadTriviaQuestions()
    {
        string[] customHeaders = null;
        var validateSsl = true;
        var error = this.Request(OpenTDB_BASE_URL + "api.php?amount=10", customHeaders, validateSsl, HTTPClient.Method.Get);
        if (error != Error.Ok)
        {
            OnTriviaRequestError(error);
        }
    }

    private void OnTriviaRequestComplete(Result result, int response_code, string[] headers, byte[] body)
    {
        GD.Print("Load Success!");
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
            
            foreach (Dictionary r in results)
            {
                GD.Print(r);
                //GD.Print(CreateFromJsonResult(r));
            }
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
        return newQ;
    }

    private void OnTriviaRequestError(Error e)
    {
        GD.Print(e);
    }
}
