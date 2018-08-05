﻿using HaloShaderGenerator.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShaderGenerator
{
    class Application
    {
        static void Benchmark()
        {
            Int64 size = 0;
            var then = DateTime.Now;
            List<Task<byte[]>> tasks = new List<Task<byte[]>>();
            for (int i = 0; i < 1000; i++)
            {
                var task = ShaderGenerator.GenerateAsync(
                ShaderStage.Albedo,
                Albedo.Two_Change_Color,
                Bump_Mapping.Off,
                Alpha_Test.None,
                Specular_Mask.No_Specular_Mask,
                Material_Model.None,
                Environment_Mapping.None,
                Self_Illumination.Off,
                Blend_Mode.Opaque,
                Parallax.Off,
                Misc.First_Person_Always,
                Distortion.Off,
                Soft_fade.Off
                );
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            foreach(var task in tasks)
            {
                size += task.Result.LongLength;
            }
            TimeSpan delta = DateTime.Now - then;
            Console.WriteLine(delta.TotalSeconds);
            Console.WriteLine(size);
            Console.WriteLine($"{1000.0 / delta.TotalSeconds} shaders/s");
        }

        static int Main()
        {
            Benchmark();

            int size = 0;

            foreach (var shaderstage in Enum.GetValues(typeof(ShaderStage)).Cast<ShaderStage>())
            {
                if (!ShaderGenerator.IsShaderStageSupported(shaderstage)) continue;

                var bytecode = ShaderGenerator.Generate(
                    shaderstage,
                    Albedo.Two_Change_Color,
                    Bump_Mapping.Off,
                    Alpha_Test.None,
                    Specular_Mask.No_Specular_Mask,
                    Material_Model.None,
                    Environment_Mapping.None,
                    Self_Illumination.Off,
                    Blend_Mode.Opaque,
                    Parallax.Off,
                    Misc.First_Person_Always,
                    Distortion.Off,
                    Soft_fade.Off
                );

                size += bytecode.Length;
            }

            return size;
        }
    }
}
