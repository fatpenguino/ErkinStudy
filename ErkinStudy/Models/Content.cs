using System;
using System.Collections.Generic;
using System.Text;
using ErkinStudy.Enums;

namespace ErkinStudy.Models
{
    public class Content
    {
        public Content(ContentType contentType, string text, long id, uint order)
        {
            ContentType = contentType;
            Text = text;
            Id = id;
            Order = order;
        }

        public long Id { get; set; }
        public string Text { get; set; }
        public uint Order { get; set; }
        public ContentType ContentType { get; set; }
    }
}
