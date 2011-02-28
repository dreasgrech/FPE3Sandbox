using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Settings
{
    public static class CollisionCategoriesSettings
    {
        public static Category Terrain { get; private set; }
        public static Category Sphere { get; private set; }
        public static Category Bridge { get; private set; }
        public static Category Vehicle { get; private set; }
        public static Category Platform { get; private set; }

        static CollisionCategoriesSettings()
        {
            Dictionary<string, int> values = new Dictionary<string, int>();
            using (var stream = TitleContainer.OpenStream(@"Config\collisioncategories.xml"))
            {
                var file = XDocument.Load(stream);
                foreach( var v in file.Descendants("category"))
                {
                    values.Add(v.Attribute("name").Value, Convert.ToInt32(v.Value));
                }
            }
            PopulateSettings(values);

        }

        static void PopulateSettings(Dictionary<string, int> values)
        {
            Terrain = CategoryFromNumber(values["terrain"]);
            Sphere = CategoryFromNumber(values["sphere"]);
            Bridge = CategoryFromNumber(values["bridge"]);
            Vehicle = CategoryFromNumber(values["vehicle"]);
            Platform = CategoryFromNumber(values["platform"]);
        }

        static Category CategoryFromNumber(int number)
        {
            return (Category) Enum.Parse(typeof (Category), "Cat" + number, true);
        }
    }
}
