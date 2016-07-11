using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Collections
{
  public class CollectionsTest : IDisposable
  {
    public CollectionsTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=CollectionsBackup;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Collection.DeleteAll();
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Collection.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Collection firstCollection = new Collection("Insect");
      Collection secondCollection = new Collection("Insect");

      //Assert
      Assert.Equal(firstCollection, secondCollection);
    }
    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Collection testCollection = new Collection("Pipe Wrenches");

      //Act
      testCollection.Save();
      List<Collection> result = Collection.GetAll();
      List<Collection> testList = new List<Collection>{testCollection};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Collection testCollection = new Collection("Mow the lawn");

      //Act
      testCollection.Save();
      Collection savedCollection = Collection.GetAll()[0];

      int result = savedCollection.GetId();
      int testId = testCollection.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      //Arrange
      Collection testCollection = new Collection("Mow the lawn");
      testCollection.Save();

      //Act
      Collection foundCollection = Collection.Find(testCollection.GetId());

      //Assert
      Assert.Equal(testCollection, foundCollection);
    }
  }
}
