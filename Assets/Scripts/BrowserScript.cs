namespace H3ml.Layout.Containers
{
    public class BrowserScript : HtmlControl
    {
        protected override void Start()
        {
            base.Start();
            Gumbo.NativeLibrary.LibraryOverride = "gumbo.dll";
            open_page("http://www.litehtml.com/");
        }
    }
}
