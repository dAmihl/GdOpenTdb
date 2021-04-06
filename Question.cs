using Godot;
using Godot.Collections;
using System;
using Array = Godot.Collections.Array;
public class Question : Godot.Object
{

    public enum Type
    {
        TRUE_FALSE,
        MULTIPLE_CHOICE
    }

    public enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    public String questionString { set; get; } = "Ques";
    public String category { get; set; } = "Cat";
    public bool isRightQuestion { set; get; } = false;

    public string typeString { set; get; } = "boolean";
    public string difficultyString { set; get; } = "easy";

    public Type questionType { set; get; } = Type.TRUE_FALSE;
    public Difficulty questionDifficulty { set; get; } = Difficulty.EASY;

    public String correctAnswer { set; get; }
    public Array<String> wrongAnswers { set; get; }

    public static Type GetTypeFromString(String typeString)
    {
        if (typeString.Equals(OpenTdbHTTP.Type.TrueFalse))
        {
            return Type.TRUE_FALSE;
        }

        if (typeString.Equals(OpenTdbHTTP.Type.MultipleChoice))
        {
            return Type.MULTIPLE_CHOICE;
        }

        return Type.MULTIPLE_CHOICE;
    }

    public static Difficulty GetDifficultyFromString(String difficultyString)
    {
        if (difficultyString.Equals(OpenTdbHTTP.Difficulty.Easy))
        {
            return Difficulty.EASY;
        }
        
        if (difficultyString.Equals(OpenTdbHTTP.Difficulty.Medium))
        {
            return Difficulty.MEDIUM;
        }
        if (difficultyString.Equals(OpenTdbHTTP.Difficulty.Hard))
        {
            return Difficulty.HARD;
        }

        return Difficulty.HARD;
    }

    public override string ToString()
    {
        return "Question: [" + questionString + "] in Category [" + category + "]";
    }

}
