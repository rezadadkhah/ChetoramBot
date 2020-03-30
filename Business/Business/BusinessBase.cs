using Business.Common.Enums;
using Business.Common.Exceptions;
using Business.Resources;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Business.Business
{
    public abstract class BusinessBase<T>
    {
        public T Result { get; internal set; }
        public string ResultMessage { get; private set; }
        public bool IsSuccessful { get; private set; }
        public bool Continue { get; set; }

        public BusinessBase()
        {
            Initialize();
        }

        private void Initialize()
        {
            IsSuccessful = true;
            ResultMessage = Messages.BusinessSuccessful;
            Context = new BotDbContext();
            Continue = true;
        }

        public static BotDbContext Context;
        public virtual void Validate() { return; }
        public abstract void Execute();

        internal void CreateInvlidResult(string message)
        {
            ResultMessage = message;
            IsSuccessful = false;
            return;
        }
        internal void ReturnInvalidResult(Types.ExceptionTypes exceptionType)
        {
            if (exceptionType == Types.ExceptionTypes.ValidationException)
                throw new ValidationException();
            if (exceptionType == Types.ExceptionTypes.InvalidResultException)
                throw new InvalidResultException();
            throw new Exception();
        }

        public void Run()
        {
            try
            {
                this.Validate();
                if (!Continue) return;
                this.Execute();
            }
            catch (Common.Exceptions.ValidationException)
            {
                this.CreateInvlidResult("User Parameters Are Invalid!");
                return;
            }
            catch (InvalidResultException)
            {
                this.CreateInvlidResult("Business Is Unsuccessful!");
                return;
            }
            catch
            {
                this.CreateInvlidResult("Operation Failed!");
                return;
            }
        }
    }

    
}