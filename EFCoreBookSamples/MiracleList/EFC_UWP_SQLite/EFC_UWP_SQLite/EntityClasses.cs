using System;
using System.Collections.Generic;

namespace EFC_UWP_SQLite
{
 /// <summary>
 /// Entity class for tasks
 /// </summary>
 public class Task
 {
  // Basic properties
  public int TaskID { get; set; } // PK
  public string Title { get; set; } //  TEXT
  public DateTime Date { get; set; } // DateTime

  // Navigation properties
  public List<TaskDetail> Details { get; set; } = new List<TaskDetail>();

  public string View { get { return Date.ToString("d") + ": " + Title; } }
 }

 /// <summary>
 /// Entity class for subtasks
 /// </summary>
 public class TaskDetail
 {
  // Basic properties
  public int TaskDetailID { get; set; } // PK
  public string Text { get; set; }

  // Navigation properties
  public Task Task { get; set; }
  public int TaskID { get; set; } // optional: Foreign key column for navigation relationship
 }
}
