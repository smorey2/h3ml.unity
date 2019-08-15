using H3ml.Layout;
using H3ml.Services;
using System.IO;
using UnityEngine;

namespace H3ml.Unity
{
    public class HtmlControl : container_unity
    {
        string _url;
        string _base_url;
        document _html;
        context _html_context = new context();
        int _rendered_width;
        string _cursor;
        string _clicked_url;
        HttpService _http = new HttpService();
        Vector3 _size;

        protected void Attach()
        {
            _size = GetComponent<Renderer>().bounds.size;
        }

        protected void OnPaint()
        {
            //using (var cr = CreateGraphics())
            //{
            //    var rect = cr.VisibleClipBounds;
            //    var pos = new position
            //    {
            //        width = (int)rect.Width,
            //        height = (int)rect.Height,
            //        x = (int)rect.X,
            //        y = (int)rect.Y
            //    };
            //    if (_html != null)
            //        _html.draw(cr, 0, 0, 0, pos);
            //}
        }

        public override void get_client_rect(out position client) => client = new position
        {
            width = (int)_size.x,
            height = (int)_size.y,
            depth = (int)_size.z,
            x = 0,
            y = 0
        };

        public override void on_anchor_click(string url, element el)
        {
            if (url != null)
                make_url(url, _base_url, out _clicked_url);
        }

        public override void set_cursor(string cursor)
        {
            if (cursor != null && _cursor != cursor)
            {
                _cursor = cursor;
                update_cursor();
            }
        }

        public override void import_css(out string text, string url, ref string baseurl)
        {
            baseurl = string.Empty;
            make_url(url, baseurl, out var css_url);
            load_text_file(css_url, out text);
            if (!string.IsNullOrEmpty(text))
                baseurl = css_url;
        }

        public override void set_caption(string caption) { }

        public override void set_base_url(string base_url) => _base_url = !string.IsNullOrEmpty(base_url) ? urljoin(_url, base_url) : _url;

        protected override object get_image(string url) => null;  //RawImage.FromStream(_http.load_file(url));

        public void open_page(string url)
        {
            _url = url;
            _base_url = url;
            load_text_file(url, out var html);
            _url = _http.url;
            _base_url = _http.url;
            //_browser.set_url(_url);
            _html = document.createFromString(html, this, _html_context);
            if (_html != null)
            {
                _rendered_width = (int)_size.x;
                _html.render(_rendered_width);
                //set_size_request(_html.width, _html.height);
            }
            //Refresh();
        }

        protected override void make_url(string url, string basepath, out string urlout) => urlout = string.IsNullOrEmpty(basepath) ? !string.IsNullOrEmpty(_base_url) ? urljoin(_base_url, url) : url : urljoin(basepath, url);

        protected void OnResize()
        {
            if (_html != null && _rendered_width != (int)_size.x)
            {
                _rendered_width = (int)_size.x;
                _html.media_changed();
                _html.render(_rendered_width);
                //set_size_request(_html.width, _html.height);
                //Refresh();
            }
        }

        //bool html_widget::on_button_press_event(GdkEventButton*event)

        //bool html_widget::on_button_release_event(GdkEventButton*event)

        //bool html_widget::on_motion_notify_event(GdkEventMotion*event)

        void update_cursor() { } // => Cursor = _cursor == "pointer" ? Cursors.Hand : Cursors.Default;

        void load_text_file(string url, out string out_)
        {
            var stream = _http.load_file(url);
            using (var r = new StreamReader(stream))
                out_ = r.ReadToEnd();
        }
    }
}
