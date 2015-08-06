using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BoletoNet
{
    public abstract class AbstractDetalheSegmento
    {
        private Match _match;
        public AbstractDetalheSegmento(string linha, Regex regex)
        {
            _match = regex.Match(linha);
            if(!_match.Success)
                throw new ArgumentException("Registro não equivale ao layout!");
        }

        protected string GetStr(string group)
        {
            return (_match.Groups[group] == null) ? "" : _match.Groups[group].Value;
        }
        protected DateTime GetDate(string group)
        {
            string s = GetStr(group);
            return (string.IsNullOrEmpty(s)) ?
                DateTime.MinValue : DateTime.ParseExact(s, "yyMMdd", CultureInfo.InvariantCulture);
        }
        protected int GetInt(string group)
        {
            string s = GetStr(group);
            return (string.IsNullOrEmpty(s)) ? 0 : int.Parse(s);
        }
        protected decimal GetDec(string group) { return (decimal)GetInt(group) / 100M; }
    }
}
