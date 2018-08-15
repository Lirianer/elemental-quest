using System; 
using System.Data;
using System.Xml;
using System.IO;

public class TMXReader 
{
    private XmlDataDocument document = new XmlDataDocument();

    public TMXReader(string filePath)
    {
        this.filePath = filePath;

        FileStream fs = new FileStream("product.xml", FileMode.Open, FileAccess.Read); 
        this.document.Load(fs); 
    }
}