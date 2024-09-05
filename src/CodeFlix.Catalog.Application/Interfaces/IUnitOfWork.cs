﻿namespace CodeFlix.Catalog.Application.Interfaces;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
}
