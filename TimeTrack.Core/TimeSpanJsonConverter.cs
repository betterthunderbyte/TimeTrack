﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TimeTrack.Core
{
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var val = reader.GetString();

            return TimeSpanConverter.ToTimeSpan(val);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}