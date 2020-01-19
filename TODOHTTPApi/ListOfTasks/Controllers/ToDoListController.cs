﻿using ListOfTasks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi;
using static ListOfTasks.Models.AllLists;

namespace ListOfTasks.Controllers
{
    public class ToDoListController : Controller
    {
        static ToDoTasksClient _client = new ToDoTasksClient();

        static ToDoListController()
        {
            GetAllList();
        }

        static async void GetAllList()
        {
            var tasks = await _client.GetTasksAsync();           
            ToDoList toDoList = new ToDoList();
            if (tasks != null)
            {
                var sort_tasks = tasks.OrderBy(a => a.TaskListId);
                string _listName = sort_tasks.First().TaskListId;

                foreach (var item in sort_tasks)
                {
                    if (item.Isactive != true)
                    {
                        if (_listName == item.TaskListId)
                        {
                            toDoList._ToDoList.Add(item);
                        }
                        else
                        {
                            toDoList.Name = _listName;
                            _AllLists.Add(toDoList);
                            toDoList = new ToDoList();
                            _listName = item.TaskListId;
                            toDoList.Name = _listName;
                            toDoList._ToDoList.Add(item);
                        }
                    }
                    all_tasks._ToDoList.Add(item);
                    if (item.Date != null)
                    {
                        planned_tasks._ToDoList.Add(item);
                    }
                    if (item.Taskimportance.ToString() == "High")
                    {
                        important_tasks._ToDoList.Add(item);
                    }
                    if (item.Date == DateTime.Now.Date)
                    {
                        today_tasks._ToDoList.Add(item);
                    }
                }
                _AllLists.Add(toDoList);
            }
        }
        // GET: ToDoList
        public IActionResult Index()
        {          
            return View(_AllLists);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string taskList)
        {
            if (taskList != null)
            {
                ToDoList toDoList = new ToDoList();
                toDoList.Name = taskList;
                _AllLists.Add(toDoList);
                return RedirectToAction(nameof(Index));
            }
            return View(taskList);
        }

        public ActionResult Edit(string list)
        {
            if (list == null)
            {
                return NotFound();
            }
            _currentList = list;
            ViewBag.Key = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string name, string key)
        {
            var item = _AllLists.FirstOrDefault(a => a.Name == _currentList);
            item.Name = name;
            if (item._ToDoList != null)
            {
                foreach (var t in item._ToDoList)
                {
                    t.TaskListId = name;
                    await _client.PutToDoTaskAsync(t.ToDoTaskId, t);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(string key)
        {
            ViewBag.Key = key;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string key)
        {
            var item = _AllLists.FirstOrDefault(a=>a.Name==key);
            if (item._ToDoList != null)
            {
                foreach (var t in item._ToDoList)
                {
                    await _client.DeleteToDoTaskAsync(t.ToDoTaskId);
                }
            }
            _AllLists.Remove(item);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddTask(string list)
        {
            _currentList = list;
            ViewData["ListId"] = list;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Додаємо задачу
        public IActionResult AddTask(ToDoTask task)
        {
            ViewData["ListId"] = _currentList;
            task.CreateDate = DateTime.Now.ToString();
            //task.TaskListId = _currentList;
            if (ModelState.IsValid)
            {
                if (task.IsMultipleTask != true)
                {
                    var item = _AllLists.FirstOrDefault(a => a.Name == _currentList);
                    foreach (var t in item._ToDoList)
                    {
                        if (t.Title == task.Title)
                        {
                            ModelState.AddModelError("", "Error double");
                            return View(task);
                        }
                    }
                }
                _client.PostToDoTaskAsync(task);
                var temp = _AllLists.FirstOrDefault(a => a.Name == _currentList);
                temp._ToDoList.Add(task);
                all_tasks._ToDoList.Add(task);
                if(task.Date!=null)
                {
                    planned_tasks._ToDoList.Add(task);
                }
                if (task.Taskimportance.ToString() == "High")
                {
                    important_tasks._ToDoList.Add(task);
                }
                if (task.Date == DateTime.Now.Date)
                {
                    today_tasks._ToDoList.Add(task);
                }
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Incorrect");
            return View(task);
        }

        public IActionResult Tasks(string list, string sort)
        {
            _currentList = list;
            ViewData["list"] = list;
            ViewData["title"] = sort == "title" ? "title_desc" : "title";
            ViewData["description"] = sort == "description" ? "description_desc" : "description";
            ViewData["taskimportance"] = sort == "taskimportance" ? "taskimportance_desc" : "taskimportance";
            ViewData["date"] = sort == "date" ? "date_desc" : "date";
            ViewData["isactive"] = sort == "isactive" ? "isactive_desc" : "isactive";
            ViewData["CreateDate"] = sort == "CreateDate" ? "CreateDate_desc" : "CreateDate";
            var Task_list = _AllLists.FirstOrDefault(a => a.Name == list);
            var task = SortOrder(sort, Task_list._ToDoList);
            return View(task);
        }
        public async Task<IActionResult> Complet(int id)
        { 
            var tasks = _AllLists.FirstOrDefault(a => a.Name == _currentList);
            var item = tasks._ToDoList.Find(a => a.ToDoTaskId == id);
            all_tasks._ToDoList.Remove(item);
            planned_tasks._ToDoList.Remove(item);
            important_tasks._ToDoList.Remove(item);
            today_tasks._ToDoList.Remove(item);
            item.IsDeleted = true;
            item.Isactive = true;
            await _client.PutToDoTaskAsync(id, item);
            tasks._ToDoList.Remove(item);
            all_tasks._ToDoList.Add(item);
            planned_tasks._ToDoList.Add(item);
            important_tasks._ToDoList.Add(item);
            today_tasks._ToDoList.Add(item);
            return RedirectToAction("Tasks", "ToDoList", new { list = _currentList });
        }
        public IActionResult SmartTasks(string smart, string hide = "show")
        {
            ViewData["HideComolet"] = hide == "hide" ? "show" : "hide";
            ViewData["SmartName"] = smart;
            var all = all_tasks;
            var planned = planned_tasks;
            var impor = important_tasks;
            var today = today_tasks;
            if (hide=="hide")
            {
                all._ToDoList = all_tasks._ToDoList.Where(a => a.Isactive == false).ToList();
                planned._ToDoList = planned_tasks._ToDoList.Where(a => a.Isactive == false).ToList();
                impor._ToDoList = important_tasks._ToDoList.Where(a => a.Isactive == false).ToList();
                today._ToDoList = today_tasks._ToDoList.Where(a => a.Isactive == false).ToList();
            }
            switch(smart)
                {
                case "all":
                    return View(all);
                case "planned":
                    return View(planned);
                case "important":
                    return View(impor);
                case "today":
                    return View(today);
            }
            return Redirect("Index");
        }
        public IActionResult Search(string sort, string title_search)
        {
            ViewData["CurrentFilter"] = title_search;
            if (!String.IsNullOrEmpty(title_search))
            {
                ViewData["title"] = sort == "title" ? "title_desc" : "title";
                ViewData["description"] = sort == "description" ? "description_desc" : "description";
                ViewData["taskimportance"] = sort == "taskimportance" ? "taskimportance_desc" : "taskimportance";
                ViewData["date"] = sort == "date" ? "date_desc" : "date";
                ViewData["isactive"] = sort == "isactive" ? "isactive_desc" : "isactive";
                ViewData["CreateDate"] = sort == "CreateDate" ? "CreateDate_desc" : "CreateDate";
                var search_task = all_tasks._ToDoList.Where(a => a.Title.ToLower().Contains(title_search.ToLower()) && a.Isactive==true);
                search_task = SortOrder(sort, search_task.ToList());
                return View(search_task.ToList());
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult DeleteTask(int id)
        {
            var task = all_tasks._ToDoList.FirstOrDefault(a => a.ToDoTaskId == id);
            return View(task);
        }
        [HttpPost]
        public IActionResult DeleteTask(int id, bool del_mult)
        {
            var item = _AllLists.FirstOrDefault(a => a.Name == _currentList);
            var del_task = item._ToDoList.Find(a => a.ToDoTaskId == id);   
            if (del_mult)
            {         
                foreach (var t in item._ToDoList)
                {
                    if ((t.IsMultipleTask == true && t.Title == del_task.Title))
                    {  
                        _client.DeleteToDoTaskAsync(t.ToDoTaskId);
                    }
                }

                item._ToDoList.RemoveAll(a => a.IsMultipleTask == true && a.Title == del_task.Title);
                all_tasks._ToDoList.RemoveAll(a => a.IsMultipleTask == true && a.Title == del_task.Title);
                planned_tasks._ToDoList.RemoveAll(a => a.IsMultipleTask == true && a.Title == del_task.Title);
                important_tasks._ToDoList.RemoveAll(a => a.IsMultipleTask == true && a.Title == del_task.Title);
                today_tasks._ToDoList.RemoveAll(a => a.IsMultipleTask == true && a.Title == del_task.Title);

                return RedirectToAction("Tasks", "ToDoList", new { list = _currentList });
            }
            else
            {
                _client.DeleteToDoTaskAsync(del_task.ToDoTaskId);
                item._ToDoList.Remove(del_task);
                all_tasks._ToDoList.Remove(del_task);
                planned_tasks._ToDoList.Remove(del_task);
                important_tasks._ToDoList.Remove(del_task);
                today_tasks._ToDoList.Remove(del_task);
            }
            return RedirectToAction("Tasks", "ToDoList", new { list = _currentList });
        }
        public IActionResult EditTask(int id)
        {
            var edit_task = _AllLists.FirstOrDefault(a => a.Name == _currentList)._ToDoList.Find(a => a.ToDoTaskId == id);
            if (edit_task != null)
            {
                ViewBag.ListName = edit_task.TaskListId;
                return View(edit_task);
            }
            return RedirectToAction("Tasks", "ToDoList", new { list = _currentList });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(int id, ToDoTask task)
        {
            var edit_task = _AllLists.FirstOrDefault(a => a.Name == _currentList)._ToDoList.Find(a => a.ToDoTaskId == id);
            var lists = _AllLists.FirstOrDefault(a => a.Name == _currentList);
            lists._ToDoList.Remove(edit_task);
            all_tasks._ToDoList.Remove(edit_task);
            planned_tasks._ToDoList.Remove(edit_task);
            important_tasks._ToDoList.Remove(edit_task);
            today_tasks._ToDoList.Remove(edit_task);
            task.TaskListId = _currentList;
            task.CreateDate = DateTime.Now.ToString();
            lists._ToDoList.Add(task);
            all_tasks._ToDoList.Add(edit_task);
            planned_tasks._ToDoList.Add(edit_task);
            important_tasks._ToDoList.Add(edit_task);
            today_tasks._ToDoList.Add(edit_task);
            await _client.PutToDoTaskAsync(id, task);
            return RedirectToAction("Tasks", "ToDoList", new { list = _currentList });
        }
        public List<ToDoTask> SortOrder(string sort, List<ToDoTask> toDoTasks)
        {
            switch (sort)
            {
                case "title":
                    toDoTasks = toDoTasks.OrderBy(s => s.Title).ToList();
                    break;
                case "title_desc":
                    toDoTasks = toDoTasks.OrderByDescending(s => s.Title).ToList();
                    break;
                case "description":
                    toDoTasks = toDoTasks.OrderBy(s => s.Description).ToList();
                    break;
                case "description_desc":
                    toDoTasks = toDoTasks.OrderByDescending(s => s.Description).ToList();
                    break;
                case "taskimportance":
                    toDoTasks = toDoTasks.OrderBy(s => s.Taskimportance).ToList();
                    break;
                case "taskimportance_desc":
                    toDoTasks = toDoTasks.OrderByDescending(s => s.Taskimportance).ToList();
                    break;
                case "date":
                    toDoTasks = toDoTasks.OrderBy(s => s.Date).ToList();
                    break;
                case "date_desc":
                    toDoTasks = toDoTasks.OrderByDescending(s => s.Date).ToList();
                    break;
                case "isactive":
                    toDoTasks = toDoTasks.OrderBy(s => s.Isactive).ToList();
                    break;
                case "isactive_desc":
                    toDoTasks = toDoTasks.OrderByDescending(s => s.Isactive).ToList();
                    break;
                case "CreateDate":
                    toDoTasks = toDoTasks.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "CreateDate_desc":
                    toDoTasks = toDoTasks.OrderByDescending(s => s.CreateDate).ToList();
                    break;
            }
            return toDoTasks;
        }
    }
}