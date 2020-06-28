using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Abacuza.Common.DataAccess
{
    /// <summary>
    /// Represents that the implemented classes are data access objects that perform
    /// CRUD operations on the given entity type.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDataAccessObject : IDisposable
    {
        /// <summary>
        /// Gets the entity by specified identifier asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the entity.</typeparam>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The task which performs the data retrieval operation.</returns>
        Task<TObject> GetByIdAsync<TObject>(Guid id) where TObject : IHasGuidId;

        /// <summary>
        /// Gets all of the entities asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the entity.</typeparam>
        /// <returns>The task which performs the data retrieval operation, and after
        /// the operation has completed, would return a list of retrieved entities.
        /// </returns>
        Task<IEnumerable<TObject>> GetAllAsync<TObject>() where TObject : IHasGuidId;

        /// <summary>
        /// Adds the given entity asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>The task which performs the adding operation.</returns>
        Task AddAsync<TObject>(TObject entity) where TObject : IHasGuidId;

        /// <summary>
        /// Updates the entity by the specified identifier asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the entity.</typeparam>
        /// <param name="id">The identifier of the entity to be updated.</param>
        /// <param name="entity">The entity which contains the updated value.</param>
        /// <returns>The task which performs the updating operation.</returns>
        Task UpdateByIdAsync<TObject>(Guid id, TObject entity) where TObject : IHasGuidId;

        /// <summary>
        /// Finds the entities which match the specified criteria that is defined by the given specification asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the entity.</typeparam>
        /// <param name="expr">The expression which defines the matching criteria.</param>
        /// <returns>The task which performs the data retrieval operation, and after the operation
        /// has completed, would return a list of entities that match the specified criteria.</returns>
        Task<IEnumerable<TObject>> FindBySpecificationAsync<TObject>(Expression<Func<TObject, bool>> expr) where TObject : IHasGuidId;

        /// <summary>
        /// Deletes the entity by specified identifier asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the entity.</typeparam>
        /// <param name="id">The identifier which represents the entity that is going to be deleted.</param>
        /// <returns>The task which performs the deletion operation.</returns>
        Task DeleteByIdAsync<TObject>(Guid id) where TObject : IHasGuidId;
    }
}
