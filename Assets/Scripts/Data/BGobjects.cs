using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGobjects
{
[System.Serializable]
public class AttributeResPlayer{
    private int id = 0;
    private string name = "";
    private int dato  = 0;
    private string tipodato  = "";
    private string fecha = "";

        public AttributeResPlayer(int Id, string Name, int Dato, string Tipodato, string Fecha)
        {
            id = Id;
            name = Name;
            dato = Dato;
            tipodato = Tipodato;
            fecha = Fecha;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Dato { get => dato; set => dato = value; }
        public string Tipodato { get => tipodato; set => tipodato = value; }
        public string Fecha { get => fecha; set => fecha = value; }
    }


[System.Serializable]
public class AttributePlayer{
    private int id = 0;
    private string name = "";
    private string nameCategory = "";
    private int dato  = 0;
    private string tipodato  = "";
    private string fuente = "";
    private string fecha = "";

        public AttributePlayer(int Id, string Name, string NameCategory, int Dato, string Tipodato, string Fuente, string Fecha)
        {
            id = Id;
            name = Name;
            nameCategory = NameCategory;
            dato = Dato;
            tipodato = Tipodato;
            fuente = Fuente;
            fecha = Fecha;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string NameCategory { get => nameCategory; set => nameCategory = value; }
        public int Dato { get => dato; set => dato = value; }
        public string Tipodato { get => tipodato; set => tipodato = value; }
        public string Fuente { get => fuente; set => fuente = value; }
        public string Fecha { get => fecha; set => fecha = value; }
    }

    

}
