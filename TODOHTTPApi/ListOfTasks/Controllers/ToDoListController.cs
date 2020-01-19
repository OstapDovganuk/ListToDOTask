using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ListOfTasks.Models.AllLists;
using WebApi;
using ListOfTasks.Models;

namespace ListOfTasks.Controllers
{
    public class ToDoListController : Controller
    {
        ToDoTasksClient _client = new ToDoTasksClient();
        async Task<IActionResult> GetAllList()
        {
            var tasks = await _client.GetTasksAsync();           
            ToDoList toDoList = new ToDoList();
            if (tasks != null)
            {
                var sort_tasks = tasks.OrderBy(a => a.TaskListId);
                string _listName = sort_tasks.First().TaskListId;

                foreach (var item in sort_tasks)
                {
                    if(_listName==item.TaskListId)
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
            return Redirect("Index");
        }
        // GET: ToDoList
        public async Task<IActionResult> Index()
        {
            var tasks = await _client.GetTasksAsync();
            ToDoList toDoList = new ToDoList();
            if (tasks != null)
            {
                var sort_tasks = tasks.OrderBy(a => a.TaskListId);
                string _listName = sort_tasks.First().TaskListId;

                foreach (var item in sort_tasks)
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
            }
            _AllLists.Add(toDoList);
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
            var item = _AllLists.FirstOrDefault(a => a.Name == key);
            item.Name = name;
            foreach(var t in item._ToDoList)
            {
                t.TaskListId = name;
                await _client.PutToDoTaskAsync(t.ToDoTaskId, t);
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}