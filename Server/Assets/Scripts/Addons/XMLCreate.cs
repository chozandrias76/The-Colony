using System;
using System.Xml;
using UnityEngine;

public class XMLCreate : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        using (XmlWriter writer = XmlWriter.Create("fontmetrics.xml"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("fontMetrics");
            writer.WriteAttributeString("file", "FontSpriteSheet02.png");
            int x = 0;
            int locX;
            int y = 15;
            int locY;
            int width = 64;
            int height = 64;
            for (int i = 0; i < 256; i++)
            {
                locX = x*width;
                locY = y*height;
                writer.WriteStartElement("character");
                writer.WriteAttributeString("key", Convert.ToString(i));
                writer.WriteElementString("x", Convert.ToString((locX)));
                writer.WriteElementString("y", Convert.ToString(locY));
                writer.WriteElementString("width", Convert.ToString(width));
                writer.WriteElementString("height", Convert.ToString(height));
                writer.WriteEndElement();
                if (x >= 15)
                {
                    x = 0;
                    y--;
                }
                else if (x < 15)
                    x++;
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}