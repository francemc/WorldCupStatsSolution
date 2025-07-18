﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Drawing;


namespace WorldCupStats.Data.Models
{
    public class Player
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("shirt_number")]
        public int ShirtNumber { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; } = string.Empty;

        [JsonPropertyName("captain")]
        public bool Captain { get; set; }
        [JsonIgnore]
        public double XPosition { get; set; }
        [JsonIgnore]
        public double YPosition { get; set; }

        [JsonIgnore]
        public int GoalsScored { get; set; }

        [JsonIgnore]
        public int YellowCards { get; set; }
        [JsonIgnore]
        public int Apparences { get; set; }

    }
}
