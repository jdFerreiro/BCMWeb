using BCMWeb.Data.EF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace BCMWeb.Models
{

    #region ClasesPrimarias
    public abstract class ItemsData : ModuloModel, IHierarchicalEnumerable, IEnumerable
    {
        public ItemsData()
        {
        }

        public abstract IEnumerable Data { get; }

        public IEnumerator GetEnumerator()
        {
            return Data.GetEnumerator();
        }
        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return (IHierarchyData)enumeratedItem;
        }
    }
    public class ItemData : ModuloModel, IHierarchyData
    {
        public string Text { get; protected set; }
        public string NavigateUrl { get; protected set; }

        public ItemData(string text, string navigateUrl)
        {
            Text = text;
            NavigateUrl = navigateUrl;
        }

        // IHierarchyData
        bool IHierarchyData.HasChildren
        {
            get { return HasChildren(); }
        }
        object IHierarchyData.Item
        {
            get { return this; }
        }
        string IHierarchyData.Path
        {
            get { return NavigateUrl; }
        }
        string IHierarchyData.Type
        {
            get { return GetType().ToString(); }
        }
        IHierarchicalEnumerable IHierarchyData.GetChildren()
        {
            return CreateChildren();
        }
        IHierarchyData IHierarchyData.GetParent()
        {
            return null;
        }

        protected virtual bool HasChildren()
        {
            return false;
        }
        protected virtual IHierarchicalEnumerable CreateChildren()
        {
            return null;
        }
    }
    #endregion

    public class SubModulosData : ItemsData
    {
        public SubModulosData(long idModulo) : base()
        {
            IdModulo = idModulo;
        }

        public override IEnumerable Data
        {
            get
            {
                return Metodos.GetSubModulos(IdModulo).Select(x => new ModuloData(x));
                // return NorthwindDataProvider.DB.Categories.ToList().Select(c => new CategoryData(c));
            }
        }
    }
    public class ModuloData : ItemData
    {
        public tblModulo Modulo { get; protected set; }

        public ModuloData(tblModulo modulo)
            : base(modulo.Nombre, string.Empty)
        {
            Modulo = modulo;
        }

        protected override bool HasChildren()
        {
            return true;
        }
        protected override IHierarchicalEnumerable CreateChildren()
        {
            return new ChildsData(Modulo.IdModulo);
        }
    }
    public class ChildsData : ItemsData
    {
        public long ModuloID { get; protected set; }

        public ChildsData(long moduloID)
            : base()
        {
            ModuloID = moduloID;
        }

        public override IEnumerable Data
        {
            get
            {
                return Metodos.GetSubModulos(ModuloID).Select(x => new ChildData(x));
                // return NorthwindDataProvider.DB.Products.Where(p => p.CategoryID == CategoryID).ToList().Select(p => new ProductData(p));
            }
        }
    }
    public class ChildData : ItemData
    {
        public ChildData(tblModulo modulo)
            : base(modulo.Nombre, "?modId=" + modulo.IdModulo.ToString())
        {
        }
    }
}