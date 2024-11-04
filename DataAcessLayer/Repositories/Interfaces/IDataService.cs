﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entities.Movies;
using DataAcessLayer.Entities.Users;


namespace DataAcessLayer;

public interface IDataService
{
    IList<User> GetUsers();

    User GetUserById(int id);

    public TitleBasic GetTitleBasic(string id);

    IList<TitleBasic> GetTitleBasics(int page, int pagesize);

    int GetNumberOfTitleBasics();
}