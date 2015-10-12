using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GKS3_UI
{
    static class Extensions
    {
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        public static int ContainsAllItems<T>(this IList<T> listA, IList<T> listB)
        {
            var bool1 = listA.Except(listB).Any();
            var bool2 = listB.Except(listA).Any();

            int difference = listA.Except(listB).Union(listB.Except(listA)).Count();

            if (bool1 == false && bool2 == false)
            {
                return 0; //A == B
            }
            else if (bool1 == true && bool2 == false)
            {
                return 1; //A+ contains B
            }
            else if (bool1 == false && bool2 == true)
            {
                return 2; //B+ contains A
            }
            else
            {
                return -1; //Some shit
            }
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            var dynamicObject = new ExpandoObject() as IDictionary<string, Object>;
            foreach (var property in properties)
            {
                dynamicObject.Add(property.Key, property.Value);
            }
            return dynamicObject;
        }

        public static List<List<T>> ConvertFromMatrix<T>(T[,] matrix)
        {
            List<List<T>> returnList = new List<List<T>>();

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                List<T> tempList = new List<T>();
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    tempList.Add(matrix[x, y]);
                }
                returnList.Add(tempList);
            }

            return returnList;
        }

        public static T[,] ConvertToMatrix<T>(List<List<T>> list)
        {
            int maxLength = list.Max(x => x.Count);
            T[,] matrix = new T[maxLength, maxLength];

            int xCounter=0;
            int yCounter=0;
            foreach (var item in list)
            {
                foreach (var subitem in item)
                {
                    matrix[xCounter, yCounter] = subitem;
                    yCounter++;
                }
                yCounter = 0;
                xCounter++;
            }

            return matrix;
        }

        public static List<string> GetSplittedWords(string str)
        {
            return str.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
