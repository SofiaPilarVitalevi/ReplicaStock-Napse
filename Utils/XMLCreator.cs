using System;
using System.Reflection;
using System.Xml;
using System.Collections;
using System.Web;

namespace Utils
{
    public class XMLCreator
    {
        #region ATRIBUTOS
        
        private XmlDocument doc = new XmlDocument();

        #endregion

        #region MÉTODOS PÚBLICOS

        /// <summary>
        /// Genera un xml como el requerido por Napse: de tipo Lista de objeto o Objeto.
        /// </summary>
        /// <param name="napseEntityName"></param>
        /// <param name="napseEntityObject"></param>
        /// <returns></returns>
        public string CreateXML(string napseEntityName, object napseEntityObject)
        {
            var type = napseEntityObject.GetType().Name;

            //** Valida si el objeto de entrada es de tipo lista, de ser asi genera un xml a partir de una Lista.
            if (type.Contains("List"))
            {
                doc.AppendChild(CreateEntityListXML(napseEntityName, napseEntityObject));
            }
            else
            {
                doc.AppendChild(CreateEntityXML(napseEntityName, napseEntityObject));
            }

            string request = HttpUtility.HtmlDecode(doc.OuterXml);
            
            return request;
        }

        #endregion

        #region MÉTODOS PRIVADOS

        /// <summary>
        /// Genera un XmlElement a partir de un objeto lista.
        /// </summary>
        /// <param name="napseEntityName"></param>
        /// <param name="napseEntityList"></param>
        /// <returns></returns>
        private XmlElement CreateEntityListXML(string napseEntityName, object napseEntityList)
        {
            try
            {
                //** Crear elementos del nivel: 'type = List'
                XmlElement listElement = napseEntityName == "alias" ? doc.CreateElement(napseEntityName + "es") : doc.CreateElement(napseEntityName + 's');
                listElement.SetAttribute("type", "list");

                foreach (var entity in (IEnumerable)napseEntityList)
                {
                    string text = HttpUtility.HtmlDecode(CreateEntityXML(napseEntityName, entity).OuterXml);
                    XmlText objText = doc.CreateTextNode(text);
                    listElement.AppendChild(objText);
                }

                return listElement;
            }
            catch (Exception e)
            {
                throw new Exception("Error al crear el xml de la entidad " + napseEntityName + ": " + e.Message);
            }
        }

        /// <summary>
        /// Genera un XmlElement a partir de un objeto.
        /// </summary>
        /// <param name="napseEntityName"></param>
        /// <param name="napseEntity"></param>
        /// <returns></returns>
        private XmlElement CreateEntityXML(string napseEntityName, object napseEntity)
        {
            try
            {
                //** Crear elementos del nivel: 'type = bean'
                XmlElement beanElement = doc.CreateElement(napseEntityName);
                beanElement.SetAttribute("type", "bean");

                PropertyInfo[] propiedades = napseEntity.GetType().GetProperties();
                foreach (PropertyInfo propiedad in propiedades)
                {
                    var propType = propiedad.PropertyType.Name;
                    if (propType.Contains("List"))
                    {
                        beanElement.AppendChild(CreateEntityListXML(propiedad.Name, propiedad.GetValue(napseEntity)));
                    }
                    else
                    {
                        XmlElement objElement = doc.CreateElement(propiedad.Name);
                        beanElement.AppendChild(objElement);

                        try
                        {
                            string value = propiedad.GetValue(napseEntity).ToString();

                            if (propiedad.GetValue(napseEntity).ToString().Contains("False") || propiedad.GetValue(napseEntity).ToString().Contains("True"))
                            {
                                value = value.ToLower();
                            }

                            XmlText objText = doc.CreateTextNode(value);

                            objElement.AppendChild(objText);
                        }
                        catch (NullReferenceException)
                        { 
                        
                        }
                    }
                }

                return beanElement;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion
    }
}
