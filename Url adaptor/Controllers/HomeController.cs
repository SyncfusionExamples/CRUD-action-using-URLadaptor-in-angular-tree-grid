using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UrlAdaptor.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System.Collections;
using Syncfusion.EJ2.Linq;
using static UrlAdaptor.Controllers.HomeController;


namespace UrlAdaptor.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public ActionResult UrlDatasource1([FromBody] DataManagerRequest dm)
        {
            IEnumerable DataSource = TreeGridItems.GetSelfData();
            DataOperations operation = new DataOperations();

            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search 
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting 
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering 
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<TreeGridItems>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging 
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
             return dm.RequiresCounts ? Ok(new { result = DataSource, count }) : Ok(DataSource);

        }
          
        public ActionResult Update([FromBody] ICRUDModel value)
        {
            if (value != null)
            {
                var val = TreeGridItems.GetSelfData().Where(ds => ds.TaskId == value.value.TaskId).FirstOrDefault();
                val.TaskName = value.value.TaskName;
                val.Duration = value.value.Duration;
                return Json(val);
            }
            else return Json(null);

        }

        public ActionResult Insert([FromBody] ICRUDModel value, int rowIndex)
        {
            var i = 0;
            if (value.Action == "insert")
            {
                rowIndex = value.relationalKey;
            }
            Random ran = new Random();
            int a = ran.Next(100, 1000);
            
            for (; i < TreeGridItems.GetSelfData().Count; i++)
            {
                if (TreeGridItems.GetSelfData()[i].TaskId == rowIndex)
                {
                    value.value.ParentId = rowIndex;
                    if (TreeGridItems.GetSelfData()[i].isParent == false)
                    {
                        TreeGridItems.GetSelfData()[i].isParent = true;
                    }
                    break;

                }
            }
            i += FindChildRecords(rowIndex);
            TreeGridItems.GetSelfData().Insert(i, value.value);

            return Json(value.value);
        }

        public void InsertAtTop([FromBody] ICRUDModel value, int rowIndex)
        {
            var i = 0;
            for (; i < TreeGridItems.GetSelfData().Count; i++)
            {
                if (TreeGridItems.GetSelfData()[i].TaskId == rowIndex)
                {
                    break;

                }
            }
            i += FindChildRecords(rowIndex);
            TreeGridItems.GetSelfData().Insert(i - 1, value.value);
        }

       public int FindChildRecords(int? id)
        {
            var count = 0;
            for (var i = 0; i < TreeGridItems.GetSelfData().Count; i++)
            {
                if (TreeGridItems.GetSelfData()[i].ParentId == id)
                {
                    count++;
                    count += FindChildRecords(TreeGridItems.GetSelfData()[i].TaskId);
                }
            }
            return count;
        }
        public void Remove([FromBody] ICRUDModel value)
        {
            if (value.Key != null)
            {
                // TreeGridItems value = key;
                TreeGridItems.GetSelfData().Remove(TreeGridItems.GetSelfData().Where(ds => ds.TaskId == double.Parse(value.Key.ToString())).FirstOrDefault());
            }

        }

      public class CustomBind : TreeGridItems
        {
            public TreeGridItems ParentId;
        }

        public class ICRUDModel
        {
            public TreeGridItems value;

 
            public int relationalKey { get; set; }
            public string Action { get; set; }

           
            public object Key { get; set; }
           
        }
        
       
    }
    public class TreeGridItems
    {
        public TreeGridItems() { }
        public int TaskId { get; set; }



        public string TaskName { get; set; }
        public int Duration { get; set; }
        public int? ParentId { get; set; }
        public bool? isParent { get; set; }


        public static List<TreeGridItems> BusinessObjectCollection = new List<TreeGridItems>();



        public static List<TreeGridItems> GetSelfData()
        {
            if (BusinessObjectCollection.Count == 0)
            {
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 1,
                    TaskName = "Parent Task 1",
                    Duration = 10,
                    ParentId = null,
                    isParent = true
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 2,
                    TaskName = "Child task 1",
                    Duration = 4,
                    ParentId = 1,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 3,
                    TaskName = "Child task 2",
                    Duration = 214,
                    ParentId = 1,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 4,
                    TaskName = "Child task 3",
                    Duration = 42,
                    ParentId = 1,
                    isParent = false
                });


                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 5,
                    TaskName = "Parent Task 2",
                    Duration = 10,
                    ParentId = null,
                    isParent = true
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 6,
                    TaskName = "Child task 4",
                    Duration = 4,
                    ParentId = 5,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 7,
                    TaskName = "Child task 5",
                    Duration = 4,
                    ParentId = 5,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 8,
                    TaskName = "Child task 6",
                    Duration = 4,
                    ParentId = 5,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 9,
                    TaskName = "Child task 7",
                    Duration = 4,
                    ParentId = 5,
                    isParent = false
                });



                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 10,
                    TaskName = "Child Task 8",
                    Duration = 10,
                    ParentId = 5,
                    isParent = false
                });

                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 11,
                    TaskName = "Parent Task 3",
                    Duration = 10,
                    ParentId = null,
                    isParent = true
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 12,
                    TaskName = "Child task 9",
                    Duration = 4,
                    ParentId = 11,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 13,
                    TaskName = "Child task 10",
                    Duration = 214,
                    ParentId = 11,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 14,
                    TaskName = "Child task 11",
                    Duration = 42,
                    ParentId = 11,
                    isParent = false
                });


                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 15,
                    TaskName = "Parent Task 4",
                    Duration = 10,
                    ParentId = null,
                    isParent = true
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 16,
                    TaskName = "Child task 12",
                    Duration = 4,
                    ParentId = 15,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 17,
                    TaskName = "Child task 13",
                    Duration = 4,
                    ParentId = 15,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 18,
                    TaskName = "Child task 14",
                    Duration = 4,
                    ParentId = 15,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 19,
                    TaskName = "Child task 15",
                    Duration = 4,
                    ParentId = 15,
                    isParent = false
                });



                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 20,
                    TaskName = "Child Task 16",
                    Duration = 10,
                    ParentId = 15,
                    isParent = false
                });

                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 21,
                    TaskName = "Parent Task 5",
                    Duration = 10,
                    ParentId = null,
                    isParent = true
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 22,
                    TaskName = "Child task 17",
                    Duration = 4,
                    ParentId = 21,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 23,
                    TaskName = "Child task 18",
                    Duration = 214,
                    ParentId = 21,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 24,
                    TaskName = "Child task 19",
                    Duration = 42,
                    ParentId = 21,
                    isParent = false
                });


                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 25,
                    TaskName = "Parent Task 6",
                    Duration = 10,
                    ParentId = null,
                    isParent = true
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 26,
                    TaskName = "Child task 20",
                    Duration = 4,
                    ParentId = 25,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 27,
                    TaskName = "Child task 21",
                    Duration = 4,
                    ParentId = 25,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 28,
                    TaskName = "Child task 22",
                    Duration = 4,
                    ParentId = 25,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 29,
                    TaskName = "Child task 23",
                    Duration = 4,
                    ParentId = 25,
                    isParent = false
                });



                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 30,
                    TaskName = "Child Task 24",
                    Duration = 10,
                    ParentId = 25,
                    isParent = false
                });

                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 31,
                    TaskName = "Parent Task 25",
                    Duration = 10,
                    ParentId = null,
                    isParent = true
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 32,
                    TaskName = "Child task 26",
                    Duration = 4,
                    ParentId = 31,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 33,
                    TaskName = "Child task 27",
                    Duration = 214,
                    ParentId = 31,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 34,
                    TaskName = "Child task 28",
                    Duration = 42,
                    ParentId = 31,
                    isParent = false
                });


                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 35,
                    TaskName = "Parent Task 8",
                    Duration = 10,
                    ParentId = null,
                    isParent = true
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 36,
                    TaskName = "Child task 29",
                    Duration = 4,
                    ParentId = 35,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 37,
                    TaskName = "Child task 30",
                    Duration = 4,
                    ParentId = 35,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 38,
                    TaskName = "Child task 31",
                    Duration = 4,
                    ParentId = 35,
                    isParent = false
                });
                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 39,
                    TaskName = "Child task 32",
                    Duration = 4,
                    ParentId = 35,
                    isParent = false
                });



                BusinessObjectCollection.Add(new TreeGridItems()
                {
                    TaskId = 40,
                    TaskName = "Child Task 33",
                    Duration = 10,
                    ParentId = 35,
                    isParent = false
                });
            }



            return BusinessObjectCollection;
        }
    }
}