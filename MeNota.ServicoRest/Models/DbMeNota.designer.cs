﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeNota.ServicoRest.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="DataSource")]
	public partial class DbMeNotaDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertUsuario(Usuario instance);
    partial void UpdateUsuario(Usuario instance);
    partial void DeleteUsuario(Usuario instance);
    partial void InsertGrupoUsuario(GrupoUsuario instance);
    partial void UpdateGrupoUsuario(GrupoUsuario instance);
    partial void DeleteGrupoUsuario(GrupoUsuario instance);
    partial void InsertGrupo(Grupo instance);
    partial void UpdateGrupo(Grupo instance);
    partial void DeleteGrupo(Grupo instance);
    #endregion
		
		public DbMeNotaDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["DataSourceConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DbMeNotaDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbMeNotaDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbMeNotaDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbMeNotaDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Usuario> Usuarios
		{
			get
			{
				return this.GetTable<Usuario>();
			}
		}
		
		public System.Data.Linq.Table<GrupoUsuario> GrupoUsuarios
		{
			get
			{
				return this.GetTable<GrupoUsuario>();
			}
		}
		
		public System.Data.Linq.Table<Grupo> Grupos
		{
			get
			{
				return this.GetTable<Grupo>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Usuario")]
	public partial class Usuario : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Nome;
		
		private string _Url;
		
		private EntitySet<GrupoUsuario> _GrupoUsuarios;
		
		private EntitySet<Grupo> _Grupos;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNomeChanging(string value);
    partial void OnNomeChanged();
    partial void OnUrlChanging(string value);
    partial void OnUrlChanged();
    #endregion
		
		public Usuario()
		{
			this._GrupoUsuarios = new EntitySet<GrupoUsuario>(new Action<GrupoUsuario>(this.attach_GrupoUsuarios), new Action<GrupoUsuario>(this.detach_GrupoUsuarios));
			this._Grupos = new EntitySet<Grupo>(new Action<Grupo>(this.attach_Grupos), new Action<Grupo>(this.detach_Grupos));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Nome", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Nome
		{
			get
			{
				return this._Nome;
			}
			set
			{
				if ((this._Nome != value))
				{
					this.OnNomeChanging(value);
					this.SendPropertyChanging();
					this._Nome = value;
					this.SendPropertyChanged("Nome");
					this.OnNomeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Url", DbType="VarChar(500)")]
		public string Url
		{
			get
			{
				return this._Url;
			}
			set
			{
				if ((this._Url != value))
				{
					this.OnUrlChanging(value);
					this.SendPropertyChanging();
					this._Url = value;
					this.SendPropertyChanged("Url");
					this.OnUrlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Usuario_GrupoUsuario", Storage="_GrupoUsuarios", ThisKey="Id", OtherKey="IdUsuario")]
		internal EntitySet<GrupoUsuario> GrupoUsuarios
		{
			get
			{
				return this._GrupoUsuarios;
			}
			set
			{
				this._GrupoUsuarios.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Usuario_Grupo", Storage="_Grupos", ThisKey="Id", OtherKey="IdAdm")]
		internal EntitySet<Grupo> Grupos
		{
			get
			{
				return this._Grupos;
			}
			set
			{
				this._Grupos.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_GrupoUsuarios(GrupoUsuario entity)
		{
			this.SendPropertyChanging();
			entity.Usuario = this;
		}
		
		private void detach_GrupoUsuarios(GrupoUsuario entity)
		{
			this.SendPropertyChanging();
			entity.Usuario = null;
		}
		
		private void attach_Grupos(Grupo entity)
		{
			this.SendPropertyChanging();
			entity.Usuario = this;
		}
		
		private void detach_Grupos(Grupo entity)
		{
			this.SendPropertyChanging();
			entity.Usuario = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.GrupoUsuario")]
	public partial class GrupoUsuario : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _IdGrupo;
		
		private int _IdUsuario;
		
		private EntityRef<Usuario> _Usuario;
		
		private EntityRef<Grupo> _Grupo;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdGrupoChanging(int value);
    partial void OnIdGrupoChanged();
    partial void OnIdUsuarioChanging(int value);
    partial void OnIdUsuarioChanged();
    #endregion
		
		public GrupoUsuario()
		{
			this._Usuario = default(EntityRef<Usuario>);
			this._Grupo = default(EntityRef<Grupo>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IdGrupo", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int IdGrupo
		{
			get
			{
				return this._IdGrupo;
			}
			set
			{
				if ((this._IdGrupo != value))
				{
					if (this._Grupo.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnIdGrupoChanging(value);
					this.SendPropertyChanging();
					this._IdGrupo = value;
					this.SendPropertyChanged("IdGrupo");
					this.OnIdGrupoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IdUsuario", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int IdUsuario
		{
			get
			{
				return this._IdUsuario;
			}
			set
			{
				if ((this._IdUsuario != value))
				{
					if (this._Usuario.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnIdUsuarioChanging(value);
					this.SendPropertyChanging();
					this._IdUsuario = value;
					this.SendPropertyChanged("IdUsuario");
					this.OnIdUsuarioChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Usuario_GrupoUsuario", Storage="_Usuario", ThisKey="IdUsuario", OtherKey="Id", IsForeignKey=true)]
		internal Usuario Usuario
		{
			get
			{
				return this._Usuario.Entity;
			}
			set
			{
				Usuario previousValue = this._Usuario.Entity;
				if (((previousValue != value) 
							|| (this._Usuario.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Usuario.Entity = null;
						previousValue.GrupoUsuarios.Remove(this);
					}
					this._Usuario.Entity = value;
					if ((value != null))
					{
						value.GrupoUsuarios.Add(this);
						this._IdUsuario = value.Id;
					}
					else
					{
						this._IdUsuario = default(int);
					}
					this.SendPropertyChanged("Usuario");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Grupo_GrupoUsuario", Storage="_Grupo", ThisKey="IdGrupo", OtherKey="Id", IsForeignKey=true)]
		internal Grupo Grupo
		{
			get
			{
				return this._Grupo.Entity;
			}
			set
			{
				Grupo previousValue = this._Grupo.Entity;
				if (((previousValue != value) 
							|| (this._Grupo.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Grupo.Entity = null;
						previousValue.GrupoUsuarios.Remove(this);
					}
					this._Grupo.Entity = value;
					if ((value != null))
					{
						value.GrupoUsuarios.Add(this);
						this._IdGrupo = value.Id;
					}
					else
					{
						this._IdGrupo = default(int);
					}
					this.SendPropertyChanged("Grupo");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Grupo")]
	public partial class Grupo : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Descricao;
		
		private int _IdAdm;
		
		private EntitySet<GrupoUsuario> _GrupoUsuarios;
		
		private EntityRef<Usuario> _Usuario;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnDescricaoChanging(string value);
    partial void OnDescricaoChanged();
    partial void OnIdAdmChanging(int value);
    partial void OnIdAdmChanged();
    #endregion
		
		public Grupo()
		{
			this._GrupoUsuarios = new EntitySet<GrupoUsuario>(new Action<GrupoUsuario>(this.attach_GrupoUsuarios), new Action<GrupoUsuario>(this.detach_GrupoUsuarios));
			this._Usuario = default(EntityRef<Usuario>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Descricao", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Descricao
		{
			get
			{
				return this._Descricao;
			}
			set
			{
				if ((this._Descricao != value))
				{
					this.OnDescricaoChanging(value);
					this.SendPropertyChanging();
					this._Descricao = value;
					this.SendPropertyChanged("Descricao");
					this.OnDescricaoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IdAdm", DbType="Int NOT NULL")]
		public int IdAdm
		{
			get
			{
				return this._IdAdm;
			}
			set
			{
				if ((this._IdAdm != value))
				{
					if (this._Usuario.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnIdAdmChanging(value);
					this.SendPropertyChanging();
					this._IdAdm = value;
					this.SendPropertyChanged("IdAdm");
					this.OnIdAdmChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Grupo_GrupoUsuario", Storage="_GrupoUsuarios", ThisKey="Id", OtherKey="IdGrupo")]
		internal EntitySet<GrupoUsuario> GrupoUsuarios
		{
			get
			{
				return this._GrupoUsuarios;
			}
			set
			{
				this._GrupoUsuarios.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Usuario_Grupo", Storage="_Usuario", ThisKey="IdAdm", OtherKey="Id", IsForeignKey=true)]
		internal Usuario Usuario
		{
			get
			{
				return this._Usuario.Entity;
			}
			set
			{
				Usuario previousValue = this._Usuario.Entity;
				if (((previousValue != value) 
							|| (this._Usuario.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Usuario.Entity = null;
						previousValue.Grupos.Remove(this);
					}
					this._Usuario.Entity = value;
					if ((value != null))
					{
						value.Grupos.Add(this);
						this._IdAdm = value.Id;
					}
					else
					{
						this._IdAdm = default(int);
					}
					this.SendPropertyChanged("Usuario");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_GrupoUsuarios(GrupoUsuario entity)
		{
			this.SendPropertyChanging();
			entity.Grupo = this;
		}
		
		private void detach_GrupoUsuarios(GrupoUsuario entity)
		{
			this.SendPropertyChanging();
			entity.Grupo = null;
		}
	}
}
#pragma warning restore 1591