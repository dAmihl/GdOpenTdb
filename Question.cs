using Godot;
using System;

public class Question : Godot.Object
{

    public enum Type
    {
        BOOLEAN,
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


    public Type questionType { set; get; } = Type.BOOLEAN;
    public Difficulty questionDifficulty { set; get; } = Difficulty.EASY;

    public static Question createFromResult(Array res)
    {
        Question q = new Question();
        return q;
        //q.questionString = res["question"];
    }

    public static Type GetTypeFromString(String typeString)
    {
        if (typeString.Equals("boolean"))
        {
            return Type.BOOLEAN;
        }

        if (typeString.Equals("multiple"))
        {
            return Type.MULTIPLE_CHOICE;
        }

        return Type.MULTIPLE_CHOICE;
    }

    public static Difficulty GetDifficultyFromString(String difficultyString)
    {
        if (difficultyString.Equals("easy"))
        {
            return Difficulty.EASY;
        }
        
        if (difficultyString.Equals("medium"))
        {
            return Difficulty.MEDIUM;
        }
        if (difficultyString.Equals("hard"))
        {
            return Difficulty.HARD;
        }

        return Difficulty.HARD;
    }

}
