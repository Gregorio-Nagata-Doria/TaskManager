using System;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Collections.Generic;

class Program
{

    public  class Task
    {
        public required int id { get; set; }
        public required string description { get; set; }
        public required string status { get; set; }
        public required string createdAt { get; set; }
        public required string updatedAt { get; set; }
    }

    static void Main(string[] args)
    {
 
        string jsonFile = File.ReadAllText("tasks.json");
        List<Task> tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile) ?? new List<Task>();
        Task chosenTask = tasks[0];
        string CurrentTime = DateTime.Now.ToString("h:mm:ss tt");


        void WriteList(string status = "")
        {
            if(status.Length == 0)
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine($"Id: {task.id}, Descrição: {task.description}, Status: {task.status}\n");
                } 
            }
            else
            {
                List<Task> filteredList = tasks.Where(x => x.status == status).ToList();
                foreach (var task in filteredList)
                {
                    Console.WriteLine($"Id: {task.id}, Descrição: {task.description}, Status: {task.status}\n");
                } 
            }
        }

        Task findTask() {
            int chosenTask = Convert.ToInt32(Console.ReadLine());  
            return tasks.Find(x => x.id == chosenTask);
        }

        try
        {
            switch (args[0])
            {
                case "add":

                    Console.WriteLine("Digite a descrição da nova tarefa");

                    int novoId = new Random().Next(1, 300);
                    string newDescrition = Console.ReadLine() ?? string.Empty ;

                    tasks.Add(new Task { id = novoId, description = newDescrition, status = "A FAZER", createdAt = CurrentTime, updatedAt = CurrentTime });

                    Console.WriteLine("Tarefa Criada com sucesso!");
                    break;

                case "update":

                    WriteList();

                    Console.WriteLine("Digite o id da tarefa que deseja atualizar");
                    chosenTask = findTask();

                    Console.WriteLine("Digite a nova descrição da tarefa");
                    string updateDescrition = Console.ReadLine() ?? string.Empty;

                    chosenTask.description = updateDescrition ?? throw new ArgumentException();
                    chosenTask.updatedAt = CurrentTime;

                    Console.WriteLine("Tarefa Atualizada com sucesso!"); 
                    break;

                case "delete":

                    WriteList();

                    Console.WriteLine("Digite o id da tarefa que deseja deletar");

                    tasks?.Remove(findTask());

                    Console.WriteLine("Tarefa Deletada com sucesso!");
                    break;

                case "mark-in-progress":

                    WriteList();

                    Console.WriteLine("Digite o id da tarefa que deseja colocar em progresso");

                    chosenTask = findTask() ?? tasks[0];

                    chosenTask.status = "PROGRESSO";
                    chosenTask.updatedAt = CurrentTime;

                    Console.WriteLine("Tarefa Atualizada com sucesso!");
                    break;

                case "mark-done":

                    WriteList();

                    Console.WriteLine("Digite o id da tarefa que deseja colocar como finalizada");

                    chosenTask = findTask();

                    chosenTask.status = "FINALIZADA";
                    chosenTask.updatedAt = CurrentTime;

                    Console.WriteLine("Tarefa Atualizada com sucesso!");
                    break;

                case "list":
              
                    if (args.ElementAtOrDefault(1) != null)
                    {
                        if (args[1] == "done")
                        {
                            WriteList("FINALIZADA");
                        }
                        else if (args[1] == "todo")
                        {
                            WriteList("A FAZER");

                        }
                        else if (args[1] == "in-progress")
                        {
                            WriteList("PROGRESSO");

                        }
                    }
                    else
                    {
                        WriteList();

                    }
                    break;
            }
            string serializedJson = JsonSerializer.Serialize(tasks);
            File.WriteAllText("tasks.json", serializedJson);

        }
        catch (Exception)
        {
            Console.WriteLine("Aconteceu um erro, por favor tente novamente");
        }

    }
}

