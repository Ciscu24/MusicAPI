using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repositories
{
    public class GenericRepository<T> where T : class
    {
        /// <summary>
        ///     Contexto de acceso a la base de datos
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        ///     Logger de la aplicación
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        ///     DbSet del modelo
        /// </summary>
        private DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }

        #region Métodos públicos

        /// <summary>
        ///     Comprueba si existe algún elemento
        /// </summary>
        /// <param name="function">Expresión que define el filtro a aplicar</param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> function = null)
        {
            try
            {
                if (function != null)
                    return _dbSet.Any(function);
                else
                    return _dbSet.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError("GenericRepository.Any", ex);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene un objeto "T" de base de datos a través de su Id
        /// </summary>
        /// <param name="id">Id del objeto que se va a obtener</param>
        /// <returns></returns>
        public T Get(object id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("GenericRepository.Get", ex);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el primer elemento que cumple el filtro de la expresión
        /// </summary>
        /// <param name="filter">Expresión que define el filtro a aplicar</param>
        /// <param name="includesProperties">Listado de funciones con las propiedades que se dean incluir</param>
        /// <returns></returns>
        public T GetFirst(Expression<Func<T, bool>> filter, Expression<Func<T, object>>[] includesProperties = null)
        {
            try
            {
                if(includesProperties != null)
                {
                    var objects = _dbSet.Where(filter);
                    IncludesProperties(ref objects, includesProperties);
                    return objects.FirstOrDefault();
                }
                else
                    return _dbSet.FirstOrDefault(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError("GenericRepository.GetFirst", ex);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene una lista completa
        /// </summary>
        /// <param name="includesProperties">Listado de funciones con las propiedades que se dean incluir</param>
        /// <returns></returns>
        public IQueryable<T> GetAll(Expression<Func<T, object>>[] includesProperties = null)
        {
            try
            {
                var objects = _dbSet.AsQueryable();

                if (includesProperties != null)
                    IncludesProperties(ref objects, includesProperties);

                return objects;
            }
            catch (Exception ex)
            {
                _logger.LogError("GenericRepository.GetAll", ex);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el primer elemento que cumple el filtro de la expresión.
        /// </summary>
        /// <param name="filter">Expresión que define el filtro a aplicar</param>
        /// <param name="includesProperties">Listado de funciones con las propiedades que se dean incluir</param>
        /// <returns></returns>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, Expression<Func<T, object>>[] includesProperties = null)
        {
            try
            {
                IQueryable<T> objects;

                if (filter != null)
                    objects =  _dbSet.Where(filter);
                else
                    objects = _dbSet.AsQueryable();

                if(includesProperties != null)
                    IncludesProperties(ref objects, includesProperties);

                return objects;
            }
            catch (Exception ex)
            {
                _logger.LogError("GenericRepository.GetAll", ex);
                throw;
            }
        }

        /// <summary>
        ///     Añade un elemento a la base de datos
        /// </summary>
        /// <param name="entity">Entidad a añadir</param>
        public void Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("GenericRepository.Add", ex);
                throw;
            }
        }

        /// <summary>
        ///     Actualiza un elemento de la base de datos
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("GenericRepository.Update", ex);
                throw;
            }
        }

        /// <summary>
        ///     Elimina un elemento de la base de datos
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("GenericRepository.Remove", ex);
                throw;
            }
        }

        #endregion

        #region Métodos privados

        /// <summary>
        ///     Método que aplica los includes pasados como parámetro al listado de elementos pasado como referencia
        /// </summary>
        /// <param name="list">Listado de elementos</param>
        /// <param name="includesProperties">Includes a aplicar</param>
        /// <returns></returns>
        private IQueryable<T> IncludesProperties(ref IQueryable<T> list, Expression<Func<T, object>>[] includesProperties)
        {
            try
            {
                foreach (var propertie in includesProperties)
                {
                    list = list.Include(propertie);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
