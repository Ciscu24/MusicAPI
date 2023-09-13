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
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el primer elemento que cumple el filtro de la expresión
        /// </summary>
        /// <param name="filter">Expresión que define el filtro a aplicar</param>
        /// <returns></returns>
        public virtual T GetFirst(Expression<Func<T, bool>> filter)
        {
            try
            {
                return _dbSet.FirstOrDefault(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene una lista completa
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            try
            {
                return _dbSet.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el primer elemento que cumple el filtro de la expresión.
        /// </summary>
        /// <param name="filter">Expresión que define el filtro a aplicar</param>
        /// <returns></returns>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            try
            {
                if (filter != null)
                    return _dbSet.Where(filter);
                else
                    return _dbSet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
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
                _logger.LogError(ex.StackTrace);
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
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        ///     Elimina un elemento de la base de datos
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        #endregion


    }
}
