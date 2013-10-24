using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using ServiceStack.Configuration;
using ServiceStack.OrmLite;
using ServiceStack.Common;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.WebHost.Endpoints;

namespace BAG.Menu
{
  //Request DTO
  public class Hello
  {
    public string Name { get; set; }
  }

  //Response DTO
  public class HelloResponse
  {
    public string Result { get; set; }
    public ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
  }

  //Can be called via any endpoint or format, see: http://servicestack.net/ServiceStack.Hello/
  public class HelloService : Service
  {
    public object Any(Hello request)
    {
      return new HelloResponse { Result = "Hello, " + request.Name };
    }
  }

  //REST Resource DTO
  [Route("/todos")]
  [Route("/todos/{Ids}")]
  public class Todos : IReturn<List<Todo>>
  {
    public long[] Ids { get; set; }
    public Todos(params long[] ids)
    {
      this.Ids = ids;
    }
  }

  [Route("/todos", "POST")]
  [Route("/todos/{Id}", "PUT")]
  public class Todo : IReturn<Todo>
  {
    public long Id { get; set; }
    public string Content { get; set; }
    public int Order { get; set; }
    public bool Done { get; set; }
  }

  public class TodosService : Service
  {
    public TodoRepository Repository { get; set; }  //Injected by IOC

    public object Get(Todos request)
    {
      return request.Ids.IsEmpty()
          ? Repository.GetAll()
          : Repository.GetByIds(request.Ids);
    }

    public object Post(Todo todo)
    {
      return Repository.Store(todo);
    }

    public object Put(Todo todo)
    {
      return Repository.Store(todo);
    }

    public void Delete(Todos request)
    {
      Repository.DeleteByIds(request.Ids);
    }
  }

  public class TodoRepository
  {
    List<Todo> todos = new List<Todo>();

    public List<Todo> GetByIds(long[] ids)
    {
      return todos.Where(x => ids.Contains(x.Id)).ToList();
    }

    public List<Todo> GetAll()
    {
      return todos;
    }

    public Todo Store(Todo todo)
    {
      var existing = todos.FirstOrDefault(x => x.Id == todo.Id);
      if (existing == null)
      {
        var newId = todos.Count > 0 ? todos.Max(x => x.Id) + 1 : 1;
        todo.Id = newId;
        todos.Add(todo);
      }
      else
      {
        existing.PopulateWith(todo);
      }
      return todo;
    }

    public void DeleteByIds(params long[] ids)
    {
      todos.RemoveAll(x => ids.Contains(x.Id));
    }
  }

}
