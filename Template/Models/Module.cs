using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Template.Repository;
using Thunder.Data.Pattern;
using Thunder.Extensions;

namespace Template.Models
{
    /// <summary>
    ///     M�dulos do sistema
    /// </summary>
    public class Module : Persist<Module, int>
    {
        private IList<Functionality> _allFunctionalities;

        /// <summary>
        ///     Inicializa uma nova inst�ncia da classe <see cref="Module" />.
        /// </summary>
        public Module()
        {
            Functionalities = new List<Functionality>();
            Childs = new List<Module>();
        }

        /// <summary>
        ///     Recupera ou define c�digo
        /// </summary>
        [Display(Name = "M�dulo Pai")]
        public new virtual int Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }

        /// <summary>
        ///     Recupera ou define nome do m�dulo
        /// </summary>
        [Required]
        [Display(Name = "Nome")]
        [StringLength(100)]
        public virtual string Name { get; set; }

        /// <summary>
        ///     Recupera ou define m�dulo pai
        /// </summary>
        public virtual Module Parent { get; set; }

        /// <summary>
        ///     Recupera ou define descri��o do m�dulo
        /// </summary>
        [Display(Name = "Descri��o")]
        [StringLength(100)]
        public virtual string Description { get; set; }

        /// <summary>
        ///     Recupera ou define css class
        /// </summary>
        [StringLength(50)]
        public virtual string CssClass { get; set; }

        /// <summary>
        ///     Recupera ou define ordem do m�dulo
        /// </summary>
        public virtual int Order { get; set; }

        /// <summary>
        ///     Recupera ou define funcionalidades do m�dulo
        /// </summary>
        public virtual IList<Functionality> Functionalities { get; set; }

        /// <summary>
        ///     Recupera ou define funcionalidade padr�o do m�dulo
        /// </summary>
        public virtual Functionality DefaultFunctionality
        {
            get { return Functionalities.SingleOrDefault(x => x.Default); }
        }

        /// <summary>
        ///     Recupera ou define filhos do m�dulo
        /// </summary>
        public virtual IList<Module> Childs { get; set; }

        /// <summary>
        ///     Lista todas as funcionalidades do m�dulo e de seus filhos
        /// </summary>
        /// <param name="module">
        ///     <see cref="Module" />
        /// </param>
        /// <returns></returns>
        private static IList<Functionality> AllFunctionalities(Module module)
        {
            var functionalities = module.Functionalities.ToList();

            foreach (var child in module.Childs)
            {
                functionalities.AddRange(AllFunctionalities(child));
            }

            return functionalities;
        }

        /// <summary>
        ///     Lista todas as funcionalidades do m�dulo e de seus filhos
        /// </summary>
        /// <returns></returns>
        private IList<Functionality> AllFunctionalities()
        {
            return _allFunctionalities ?? (_allFunctionalities = AllFunctionalities(this));
        }

        /// <summary>
        ///     M�dulo cont�m funcionalidade
        /// </summary>
        /// <param name="functionality">
        ///     <see cref="Functionality" />
        /// </param>
        /// <returns>Cont�m</returns>
        public virtual bool Contains(Functionality functionality)
        {
            return AllFunctionalities().Contains(functionality);
        }

        /// <summary>
        ///     M�dulo cont�m funcionalidade
        /// </summary>
        /// <param name="controllerName">Nome da controller</param>
        /// <param name="actionName">Nome da action</param>
        /// <returns>Cont�m</returns>
        public virtual bool Contains(string controllerName, string actionName)
        {
            return AllFunctionalities().Any(functionality =>
                functionality.Controller.ToLower().Equals(controllerName.ToLower()) &&
                functionality.Action.ToLower().Equals(actionName.ToLower()));
        }

        /// <summary>
        ///     Valida <see cref="Module" />
        /// </summary>
        /// <param name="moduleRepository"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public virtual bool IsValid(IModuleRepository moduleRepository, ModelStateDictionary modelState)
        {
            modelState.Remove("Parent.Id");
            modelState.Remove("Parent.Name");

            if (IsNew() && Parent.Id <= 0)
            {
                Parent = null;
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                if (moduleRepository.Exist(Id, Parent, Name))
                {
                    modelState.AddModelError("Name", "O nome do m�dulo \"{0}\" j� encontra-se cadastrado.".With(Name));    
                }
            }

            if (Functionalities != null && Functionalities.Any())
            {
                if (Functionalities.Count(x => x.Default) == 0)
                {
                    modelState.AddModelError("Functionalities", "� necess�rio definir ao menos uma funcionalidade como sendo a principal do m�dulo.");
                }

                if (Functionalities.Count(x => x.Default) > 1)
                {
                    modelState.AddModelError("Functionalities", "� permitido definir somente uma funcionalidade como a principal do m�dulo.");
                }
            }
            return modelState.IsValid;
        }
    }
}