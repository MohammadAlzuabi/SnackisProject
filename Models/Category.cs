﻿using System.Text.Json.Serialization;

namespace SnackisProject.Models
{
    public class Category
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
