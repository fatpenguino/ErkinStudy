using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Web.Models
{
    public class FolderViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public bool IsQuizGroup { get; set; }
        public IEnumerable<FolderItem> Items { get; set; } 
    }

    public class FolderItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public uint Order { get; set; }
        public long Price { get; set; }
        public string Color { get; set; }
        public FolderItemType Type { get; set; }
        public QuizType QuizType { get; set; }
    }

    public enum FolderItemType
    {
        Folder,
        Course,
        Quiz,
        Lesson
    }
}
