﻿namespace Libs.Entity
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Topic>? Topics { get; set; }
    }
}
