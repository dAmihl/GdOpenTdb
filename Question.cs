using Godot;
using System;

public class Question : Godot.Object
{

    public String questionString { set; get; } = "Ques";
    public String category { get; set; } = "Cat";
    public bool isRightQuestion { set; get; } = false;

    public string typeString { set; get; } = "boolean";
    public string difficultyString { set; get; } = "easy";

    public static Question createFromResult(Array res)
    {
        Question q = new Question();
        return q;
        //q.questionString = res["question"];
    }

}
