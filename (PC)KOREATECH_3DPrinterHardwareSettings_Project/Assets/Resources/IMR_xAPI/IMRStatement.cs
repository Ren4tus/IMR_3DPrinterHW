using TinCan;
using TinCan.LRSResponses;
using System;
using Newtonsoft.Json.Linq;
using Extensions = TinCan.Extensions;

namespace IMR
{
    public class IMRStatement
    {
        protected Agent _actor;
        protected Verb _verb;
        protected Activity _activity;
        protected ActivityDefinition _definition;
        protected Result _result;
        protected DateTime? _timestamp;
        protected Context _context;
        protected Agent _authority;
        protected Statement _statement;

        public IMRStatement()
        {
            Init();
        }

        public virtual void Init()
        {
            _statement = new Statement();
            _actor = new Agent();
            _verb = new Verb();
            _activity = new Activity();
            _definition = new ActivityDefinition();
            _result = new Result();
            _context = new Context();
            _timestamp = new DateTime();
            _authority = new Agent();
            SetAuthority();
        }

        public virtual void SetActor()
        {
            var actor = XAPIApplication.current.ActorName;
            _actor.mbox = "mailto:" + actor.Replace(" ", "") + "@google.com";
            _actor.name = actor;
            _statement.actor = _actor;

            UnityEngine.Debug.Log("SetActor "+ XAPIApplication.current.ActorName + "/"+ _statement.actor.mbox);
        }

        public virtual void SetVerb(string verb)
        {
            _verb.id = new Uri("https://www.koreatech.ac.kr/" + verb.Replace(" ", ""));
            _verb.display = new LanguageMap();
            _verb.display.Add("en-US", verb);
            _statement.verb = _verb;

            UnityEngine.Debug.Log("SetVerb " + _statement.verb.ToJSON());
        }

        public virtual void SetActivity(string activity)
        {
            _activity.id = new Uri("https://www.koreatech.ac.kr/" + activity.Replace(" ", "")).ToString();
            _statement.target = this._activity;
        }

        public virtual void SetDefinition(string def)
        {
            _definition.description = new LanguageMap();
            _definition.name = new LanguageMap();
            _definition.name.Add("en-US", (def));
            _activity.definition = _definition;
            _statement.target = this._activity;
        }

        public void SetResultExtensions(JObject jo)
        {
            _result.extensions = new Extensions(jo);
        }

        public virtual void SetTimestamp(DateTime t)
        {
            _timestamp = t;
            _statement.timestamp = _timestamp;
        }

        public virtual void SetAuthority()
        {
            _authority.name = "KoreaTech";
            _authority.mbox = "mailto:KoreaTech@koreatech.ac.kr";
            _statement.authority = _authority;
        }

        public virtual void SetAuthority(string auth = "koreatech")
        {
            _authority.name = auth;
            _authority.mbox = string.Format("mailto:{0}@koreatech.ac.kr", auth);
        }

        public virtual void SetContextExtensions(JObject jobject)
        {
            _context.extensions = new Extensions(jobject);
        }

        public virtual Statement GetStatement()
        {
            _statement.actor = _actor;
            _statement.verb = _verb;
            _statement.target = _activity;
            _statement.result = _result;
            _statement.timestamp = DateTime.Now;
            _statement.context = _context;
            _statement.authority = _authority;

            UnityEngine.Debug.Log(_statement.ToJSON());
            return _statement;
        }
    }
}