namespace Eco
{
    using System.Collections.Generic;

    public class Email
    {
        public string From
        {
            get;
            set;
        }

        public string To
        {
            get;
            set;
        }

        public List<string> Ccs
        {
            get;
            set;
        } = new List<string>();

        public string Subject
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }

        public bool BodyIsHtml
        {
            get;
            set;
        }

        public string TemplateName
        {
            get;
            set;
        }

        public string TemplateId
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string StartupName
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string FounderFullName
        {
            get;
            set;
        }
    }
}
