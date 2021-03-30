# Godot OpenTdb Plugin

WIP

* Add Plugin to Project Settings
* Add OpenTdb as a Child Node to any Node
* Then:

Example Usage:

```
extends Node2D

func _ready():
	$OpenTdb.connect("QuestionsLoaded", self, "_on_questions_loaded")
	$OpenTdb.LoadTriviaQuestions(10)
	pass

func _on_questions_loaded(questions_arr):
	print("Questions loaded in GDSCript!")
	for q in questions_arr:
		print(q)

```

