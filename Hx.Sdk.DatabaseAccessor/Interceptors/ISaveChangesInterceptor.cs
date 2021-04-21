﻿using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Sdk.DatabaseAccessor.Interceptors
{
    public interface ISaveChangesInterceptor : IInterceptor
    {
        /// <summary>
        ///     Called at the start of <see cref="M:DbContext.SaveChanges" />.
        /// </summary>
        /// <param name="eventData"> Contextual information about the <see cref="DbContext" /> being used. </param>
        /// <param name="result">
        ///     Represents the current result if one exists.
        ///     This value will have <see cref="InterceptionResult{Int32}.HasResult" /> set to <see langword="true" /> if some previous
        ///     interceptor suppressed execution by calling <see cref="InterceptionResult{Int32}.SuppressWithResult" />.
        ///     This value is typically used as the return value for the implementation of this method.
        /// </param>
        /// <returns>
        ///     If <see cref="InterceptionResult{Int32}.HasResult" /> is false, the EF will continue as normal.
        ///     If <see cref="InterceptionResult{Int32}.HasResult" /> is true, then EF will suppress the operation it
        ///     was about to perform and use <see cref="InterceptionResult{Int32}.Result" /> instead.
        ///     A normal implementation of this method for any interceptor that is not attempting to change the result
        ///     is to return the <paramref name="result" /> value passed in.
        /// </returns>
        InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result);

        /// <summary>
        ///     <para>
        ///         Called at the end of <see cref="M:DbContext.SaveChanges" />.
        ///     </para>
        ///     <para>
        ///         This method is still called if an interceptor suppressed creation of a command in <see cref="SavingChanges" />.
        ///         In this case, <paramref name="result" /> is the result returned by <see cref="SavingChanges" />.
        ///     </para>
        /// </summary>
        /// <param name="eventData"> Contextual information about the <see cref="DbContext" /> being used. </param>
        /// <param name="result">
        ///     The result of the call to <see cref="M:DbContext.SaveChanges" />.
        ///     This value is typically used as the return value for the implementation of this method.
        /// </param>
        /// <returns>
        ///     The result that EF will use.
        ///     A normal implementation of this method for any interceptor that is not attempting to change the result
        ///     is to return the <paramref name="result" /> value passed in.
        /// </returns>
        int SavedChanges(
            SaveChangesCompletedEventData eventData,
            int result);

        /// <summary>
        ///     Called when an exception has been thrown in <see cref="M:DbContext.SaveChanges" />.
        /// </summary>
        /// <param name="eventData"> Contextual information about the failure. </param>
        void SaveChangesFailed(
            DbContextErrorEventData eventData);

        /// <summary>
        ///     Called at the start of <see cref="M:DbContext.SaveChangesAsync" />.
        /// </summary>
        /// <param name="eventData"> Contextual information about the <see cref="DbContext" /> being used. </param>
        /// <param name="result">
        ///     Represents the current result if one exists.
        ///     This value will have <see cref="InterceptionResult{Int32}.HasResult" /> set to <see langword="true" /> if some previous
        ///     interceptor suppressed execution by calling <see cref="InterceptionResult{Int32}.SuppressWithResult" />.
        ///     This value is typically used as the return value for the implementation of this method.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns>
        ///     If <see cref="InterceptionResult{Int32}.HasResult" /> is false, the EF will continue as normal.
        ///     If <see cref="InterceptionResult{Int32}.HasResult" /> is true, then EF will suppress the operation it
        ///     was about to perform and use <see cref="InterceptionResult{Int32}.Result" /> instead.
        ///     A normal implementation of this method for any interceptor that is not attempting to change the result
        ///     is to return the <paramref name="result" /> value passed in.
        /// </returns>
        /// <exception cref="OperationCanceledException"> If the <see cref="CancellationToken"/> is canceled. </exception>
        Task<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     <para>
        ///         Called at the end of <see cref="M:DbContext.SaveChangesAsync" />.
        ///     </para>
        ///     <para>
        ///         This method is still called if an interceptor suppressed creation of a command in <see cref="SavingChangesAsync" />.
        ///         In this case, <paramref name="result" /> is the result returned by <see cref="SavingChangesAsync" />.
        ///     </para>
        /// </summary>
        /// <param name="eventData"> Contextual information about the <see cref="DbContext" /> being used. </param>
        /// <param name="result">
        ///     The result of the call to <see cref="M:DbContext.SaveChangesAsync" />.
        ///     This value is typically used as the return value for the implementation of this method.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns>
        ///     The result that EF will use.
        ///     A normal implementation of this method for any interceptor that is not attempting to change the result
        ///     is to return the <paramref name="result" /> value passed in.
        /// </returns>
        /// <exception cref="OperationCanceledException"> If the <see cref="CancellationToken"/> is canceled. </exception>
        Task<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Called when an exception has been thrown in <see cref="M:DbContext.SaveChangesAsync" />.
        /// </summary>
        /// <param name="eventData"> Contextual information about the failure. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task" /> representing the asynchronous operation. </returns>
        /// <exception cref="OperationCanceledException"> If the <see cref="CancellationToken"/> is canceled. </exception>
        Task SaveChangesFailedAsync(
            DbContextErrorEventData eventData,
            CancellationToken cancellationToken = default);
    }
}
