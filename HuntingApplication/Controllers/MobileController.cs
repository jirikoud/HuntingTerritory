using HuntingModel.Context;
using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Model.API;
using HuntingModel.ViewModel.CheckInModels;
using HuntingModel.ViewModel.UserPointModels;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HuntingApplication.Controllers
{
    public class MobileController : ApiController
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public const string SESSION_HEADER = "MobileSession";

        public const string SESSION_INVALID_MESSAGE = "Invalid session";
        public const string FORBIDDEN_MESSAGE = "Not allowed to this resource";
        public const string FAILED_MESSAGE = "Operation failed";
        public const string BAD_REQUEST_MESSAGE = "Invalid parameters";
        public const string NOT_FOUND_MESSAGE = "Object not found";

        private string GetSession()
        {
            IEnumerable<string> sessionList;
            if (Request.Headers.TryGetValues(SESSION_HEADER, out sessionList))
            {
                var session = sessionList.FirstOrDefault();
                return session;
            }

            #if DEBUG
            var sessionParam = Request.GetQueryNameValuePairs().
                Where(item => item.Key == "session").
                Select(item => item.Value).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(sessionParam) == false)
            {
                return sessionParam;
            }
            #endif

            return null;
        }

        [HttpGet]
        public IHttpActionResult Validation()
        {
            try
            {
                return Ok("Api is here!");
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult Login(LoginModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    UserSession userSession;
                    var result = AclUserContext.LoginUser(dataContext, model.Email, model.Password, out userSession);
                    switch (result)
                    {
                        case LoginResultEnum.Error:
                            return InternalServerError();
                        case LoginResultEnum.NotFound:
                            return NotFound();
                        case LoginResultEnum.WrongPassword:
                            return NotFound();
                        case LoginResultEnum.NotAllowed:
                            return NotFound();
                        case LoginResultEnum.Success:
                            var responseModel = new LoginResponseModel(userSession);
                            return Ok(responseModel);
                    }
                    return NotFound();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController - Login, Model = {0}", (model != null ? model.Email : "N/A"));
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult TerritoryList()
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var session = GetSession();
                    if (session == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var userSession = AclUserContext.GetUserSession(dataContext, session);
                    if (userSession == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var territoryList = TerritoryContext.GetListForUser(dataContext, userSession.AclUser);
                    var model = new TerritoryListModel(territoryList, userSession.AclUser);
                    return Ok(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult TerritoryDetail(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var session = GetSession();
                    if (session == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var userSession = AclUserContext.GetUserSession(dataContext, session);
                    if (userSession == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var territory = TerritoryContext.GetDetail(dataContext, id);
                    if (userSession.AclUser.CanViewTerritory(territory) == false)
                    {
                        return Content(HttpStatusCode.Forbidden, FORBIDDEN_MESSAGE);
                    }
                    var model = new TerritoryDetailModel(territory, userSession.AclUser);
                    return Ok(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult CreateTrack()
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var session = GetSession();
                    if (session == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var userSession = AclUserContext.GetUserSession(dataContext, session);
                    if (userSession == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var locationId = UserLocationContext.CreateTrack(dataContext, userSession.AclUserId);
                    if (locationId == null)
                    {
                        return Content(HttpStatusCode.InternalServerError, FAILED_MESSAGE);
                    }
                    return Ok(locationId);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult UserLocation(UserLocationParamModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var session = GetSession();
                    if (session == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var userSession = AclUserContext.GetUserSession(dataContext, session);
                    if (userSession == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var locationId = UserLocationContext.CreateUserLocation(dataContext, model.TrackId, model.LocationX, model.LocationY, userSession.AclUserId);
                    if (locationId == null)
                    {
                        return Content(HttpStatusCode.InternalServerError, FAILED_MESSAGE);
                    }
                    return Ok();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult UserLocationList(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var session = GetSession();
                    if (session == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var userSession = AclUserContext.GetUserSession(dataContext, session);
                    if (userSession == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var territory = TerritoryContext.GetDetail(dataContext, id);
                    if (userSession.AclUser.CanViewTerritory(territory) == false)
                    {
                        return Content(HttpStatusCode.Forbidden, FORBIDDEN_MESSAGE);
                    }
                    var userLocationList = UserLocationContext.GetUserLocationList(dataContext, territory);
                    if (userLocationList == null)
                    {
                        return Content(HttpStatusCode.InternalServerError, FAILED_MESSAGE);
                    }
                    var model = userLocationList.ConvertAll(item => new UserLocationModel(item));
                    return Ok(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult CreateUserPoint(CreateUserPointModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var session = GetSession();
                    if (session == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var userSession = AclUserContext.GetUserSession(dataContext, session);
                    if (userSession == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var territory = TerritoryContext.GetDetail(dataContext, model.TerritoryId);
                    if (userSession.AclUser.CanViewTerritory(territory) == false)
                    {
                        return Content(HttpStatusCode.Forbidden, FORBIDDEN_MESSAGE);
                    }
                    if (model.IsValid() == false)
                    {
                        return Content(HttpStatusCode.BadRequest, BAD_REQUEST_MESSAGE);
                    }
                    var updateModel = new UserPointUpdateModel(model);
                    var pointId = UserPointContext.Update(dataContext, null, updateModel, userSession.AclUserId);
                    if (pointId == null)
                    {
                        return Content(HttpStatusCode.InternalServerError, FAILED_MESSAGE);
                    }
                    return Ok(pointId.Value.ToString());
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult CreateMapItem(CreateMapItemModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var session = GetSession();
                    if (session == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var userSession = AclUserContext.GetUserSession(dataContext, session);
                    if (userSession == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var territory = TerritoryContext.GetDetail(dataContext, model.TerritoryId);
                    if (userSession.AclUser.CanUpdateTerritory(territory) == false)
                    {
                        return Content(HttpStatusCode.Forbidden, FORBIDDEN_MESSAGE);
                    }
                    var mapItemType = MapItemTypeContext.GetDetail(dataContext, model.MapItemTypeId);
                    if (mapItemType == null || mapItemType.TerritoryId != territory.Id)
                    {
                        return Content(HttpStatusCode.BadRequest, BAD_REQUEST_MESSAGE);
                    }
                    if (model.IsValid() == false)
                    {
                        return Content(HttpStatusCode.BadRequest, BAD_REQUEST_MESSAGE);
                    }
                    var pointId = MapItemContext.Create(dataContext, model, userSession.AclUserId);
                    if (pointId == null)
                    {
                        return Content(HttpStatusCode.InternalServerError, FAILED_MESSAGE);
                    }
                    return Ok(pointId.Value.ToString());
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult CheckinList(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var session = GetSession();
                    if (session == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var userSession = AclUserContext.GetUserSession(dataContext, session);
                    if (userSession == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var mapItem = MapItemContext.GetDetail(dataContext, id);
                    if (mapItem == null)
                    {
                        return Content(HttpStatusCode.NotFound, NOT_FOUND_MESSAGE);
                    }
                    if (userSession.AclUser.CanViewTerritory(mapItem.MapItemTypeRecord.Territory) == false)
                    {
                        return Content(HttpStatusCode.Forbidden, FORBIDDEN_MESSAGE);
                    }
                    var checkInList = MapItemContext.GetCheckInList(dataContext, mapItem);
                    if (checkInList == null)
                    {
                        return Content(HttpStatusCode.InternalServerError, FAILED_MESSAGE);
                    }
                    var model = checkInList.ConvertAll(item => new CheckInModel(item));
                    return Ok(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult CheckIn(CreateCheckInModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var session = GetSession();
                    if (session == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var userSession = AclUserContext.GetUserSession(dataContext, session);
                    if (userSession == null)
                    {
                        return Content(HttpStatusCode.Unauthorized, SESSION_INVALID_MESSAGE);
                    }
                    var mapItem = MapItemContext.GetDetail(dataContext, model.MapItemId);
                    if (mapItem == null)
                    {
                        return Content(HttpStatusCode.NotFound, NOT_FOUND_MESSAGE);
                    }
                    if (userSession.AclUser.CanViewTerritory(mapItem.Territory) == false)
                    {
                        return Content(HttpStatusCode.Forbidden, FORBIDDEN_MESSAGE);
                    }
                    Questionnaire questionnaire = null;
                    if (model.QuestionnaireId.HasValue)
                    {
                        questionnaire = QuestionnaireContext.GetDetail(dataContext, model.QuestionnaireId.Value);
                    }
                    if (model.IsValid(dataContext, questionnaire) == false)
                    {
                        return Content(HttpStatusCode.BadRequest, BAD_REQUEST_MESSAGE);
                    }
                    var updateModel = new CheckInUpdateModel(model, questionnaire);
                    var pointId = CheckInContext.Update(dataContext, null, updateModel, userSession.AclUser, null);
                    if (pointId == null)
                    {
                        return Content(HttpStatusCode.InternalServerError, FAILED_MESSAGE);
                    }
                    return Ok(pointId.Value.ToString());
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MobileController");
                return InternalServerError();
            }
        }

    }
}
