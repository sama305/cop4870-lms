
namespace Library.LMS.Models;

public class Module
{
	// fields/properties
	public string Name { get; set; }
	public string Desc { get; set; }
	public List<ContentItem> Contents { get; }

	public void addContentItem(ContentItem ci)
	{ Contents.Add(ci); }

	// constructor
	public Module(string name, string desc)
	{
		Name = name;
		Desc = desc;
		Contents = new List<ContentItem>();
	}

	public Module(Module prevModule)
	{
		Name = prevModule.Name;
		Desc = prevModule.Desc;
		Contents = prevModule.Contents;
	}
}

