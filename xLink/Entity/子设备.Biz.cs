using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using NewLife.Log;
using NewLife.Model;
using NewLife.Reflection;
using NewLife.Threading;
using NewLife.Web;
using XCode;
using XCode.Cache;
using XCode.Configuration;
using XCode.DataAccessLayer;
using XCode.Membership;

namespace xLink.Entity
{
    /// <summary>子设备</summary>
    public partial class SubDevice : Entity<SubDevice>
    {
        #region 对象操作
        static SubDevice()
        {
            // 累加字段
            //var df = Meta.Factory.AdditionalFields;
            //df.Add(__.ProductId);

            // 过滤器 UserModule、TimeModule、IPModule
            Meta.Modules.Add<UserModule>();
            Meta.Modules.Add<TimeModule>();
            Meta.Modules.Add<IPModule>();
        }

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew">是否插入</param>
        public override void Valid(Boolean isNew)
        {
            // 如果没有脏数据，则不需要进行任何处理
            if (!HasDirty) return;

            // 在新插入数据或者修改了指定字段时进行修正
            // 处理当前已登录用户信息，可以由UserModule过滤器代劳
            /*var user = ManageProvider.User;
            if (user != null)
            {
                if (isNew && !Dirtys[nameof(CreateUserID)]) CreateUserID = user.ID;
                if (!Dirtys[nameof(UpdateUserID)]) UpdateUserID = user.ID;
            }*/
            //if (isNew && !Dirtys[nameof(CreateTime)]) CreateTime = DateTime.Now;
            //if (!Dirtys[nameof(UpdateTime)]) UpdateTime = DateTime.Now;
            //if (isNew && !Dirtys[nameof(CreateIP)]) CreateIP = ManageProvider.UserHost;
            //if (!Dirtys[nameof(UpdateIP)]) UpdateIP = ManageProvider.UserHost;

            // 检查唯一索引
            // CheckExist(isNew, __.Code);
        }

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected internal override void InitData()
        //{
        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    if (Meta.Session.Count > 0) return;

        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化SubDevice[子设备]数据……");

        //    var entity = new SubDevice();
        //    entity.ID = 0;
        //    entity.ProductId = 0;
        //    entity.DeviceId = 0;
        //    entity.Code = "abc";
        //    entity.Name = "abc";
        //    entity.Version = "abc";
        //    entity.Vendor = "abc";
        //    entity.Model = "abc";
        //    entity.Enable = true;
        //    entity.Remark = "abc";
        //    entity.CreateUserID = 0;
        //    entity.CreateTime = DateTime.Now;
        //    entity.CreateIP = "abc";
        //    entity.UpdateUserID = 0;
        //    entity.UpdateTime = DateTime.Now;
        //    entity.UpdateIP = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化SubDevice[子设备]数据！");
        //}

        ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        ///// <returns></returns>
        //public override Int32 Insert()
        //{
        //    return base.Insert();
        //}

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnDelete()
        //{
        //    return base.OnDelete();
        //}
        #endregion

        #region 扩展属性
        /// <summary>产品</summary>
        [XmlIgnore, IgnoreDataMember]
        //[ScriptIgnore]
        public Product Product { get { return Extends.Get(nameof(Product), k => Product.FindByID(ProductId)); } }

        /// <summary>产品</summary>
        [XmlIgnore, IgnoreDataMember]
        //[ScriptIgnore]
        [DisplayName("产品")]
        [Map(__.ProductId, typeof(Product), "ID")]
        public String ProductName { get { return Product?.Name; } }
        /// <summary>设备</summary>
        [XmlIgnore, IgnoreDataMember]
        //[ScriptIgnore]
        public Device Device { get { return Extends.Get(nameof(Device), k => Device.FindByID(DeviceId)); } }

        /// <summary>设备</summary>
        [XmlIgnore, IgnoreDataMember]
        //[ScriptIgnore]
        [DisplayName("设备")]
        [Map(__.DeviceId, typeof(Device), "ID")]
        public String DeviceName { get { return Device?.Name; } }
        #endregion

        #region 扩展查询
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns>实体对象</returns>
        public static SubDevice FindByID(Int32 id)
        {
            if (id <= 0) return null;

            // 实体缓存
            if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.ID == id);

            // 单对象缓存
            return Meta.SingleCache[id];

            //return Find(_.ID == id);
        }

        /// <summary>根据编码查找</summary>
        /// <param name="code">编码</param>
        /// <returns>实体对象</returns>
        public static SubDevice FindByCode(String code)
        {
            // 实体缓存
            if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Code == code);

            return Find(_.Code == code);
        }

        /// <summary>根据设备查找</summary>
        /// <param name="deviceid">设备</param>
        /// <returns>实体列表</returns>
        public static IList<SubDevice> FindAllByDeviceId(Int32 deviceid)
        {
            // 实体缓存
            if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.DeviceId == deviceid);

            return FindAll(_.DeviceId == deviceid);
        }

        /// <summary>根据经销商、产品型号查找</summary>
        /// <param name="vendor">经销商</param>
        /// <param name="model">产品型号</param>
        /// <returns>实体列表</returns>
        public static IList<SubDevice> FindAllByVendorAndModel(String vendor, String model)
        {
            // 实体缓存
            if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.Vendor == vendor && e.Model == model);

            return FindAll(_.Vendor == vendor & _.Model == model);
        }
        #endregion

        #region 高级查询
        #endregion

        #region 业务操作
        #endregion
    }
}