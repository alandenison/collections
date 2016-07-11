using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Collections
{
  public class Collection
  {
    private int _id;
    private string _collectionName;

    public Collection(string collectionName, int Id = 0)
    {
      _id = Id;
      _collectionName = collectionName;
    }

    public override bool Equals(System.Object otherCollection)
    {
        if (!(otherCollection is Collection))
        {
          return false;
        }
        else
        {
          Collection newCollection = (Collection) otherCollection;
          bool idEquality = (this.GetId() == newCollection.GetId());
          bool descriptionEquality = (this.GetCollectionName() == newCollection.GetCollectionName());
          return (idEquality && descriptionEquality);
        }
    }

    public static Collection Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM CollectionNameTable WHERE id = @CollectionId;", conn);
      SqlParameter collectionIdParameter = new SqlParameter();
      collectionIdParameter.ParameterName = "@CollectionId";
      collectionIdParameter.Value = id.ToString();
      cmd.Parameters.Add(collectionIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCollectionId = 0;
      string foundCollectionName = null;
      while(rdr.Read())
      {
        foundCollectionId = rdr.GetInt32(0);
        foundCollectionName = rdr.GetString(1);
      }
      Collection foundCollection = new Collection(foundCollectionName, foundCollectionId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundCollection;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO CollectionNameTable (CollectionName) OUTPUT INSERTED.id VALUES (@CollectionName);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@CollectionName";
      descriptionParameter.Value = this.GetCollectionName();
      cmd.Parameters.Add(descriptionParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetCollectionName()
    {
    return _collectionName;
    }
    public void SetCollectionName(string newCollectionName)
    {
    _collectionName = newCollectionName;
    }

    public static List <Collection> GetAll()
    {
      List<Collection> allCollections = new List<Collection>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM CollectionNameTable;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int collectionId = rdr.GetInt32(0);
        string collectionName = rdr.GetString(1);
        Collection newCollection = new Collection(collectionName, collectionId);
        allCollections.Add(newCollection);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCollections;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM collectionNameTable;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
