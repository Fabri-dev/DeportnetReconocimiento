﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace DeportNetReconocimiento.Api.Data.Domain
{
    [Table("credenciales")]
    public class Credenciales
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }


        [Column("ip")]
        [StringLength(15)]
        [AllowNull]
        public string? Ip { get; set; }

        [Column("port")]
        [StringLength(5)]
        [AllowNull]
        public string? Port { get; set; }

        [Column("username")]
        [StringLength(100)]
        [AllowNull]
        public string? Username { get; set; }

        [Column("password")]
        [StringLength(100)]
        [AllowNull]
        public string? Password { get; set; }

        [Column("branch_id")]
        [StringLength(50)]
        [AllowNull]
        public string? BranchId{ get; set; }

        [Column("branch_token")]
        [StringLength(50)]
        [AllowNull]
        public string? BranchToken{ get; set; }

        [Column("current_company_member_id")]
        public string? CurrentCompanyMemberId { get; set; }

        public Credenciales() { }
        public Credenciales(
            string? ip, 
            string? port,
            string? username,
            string? password,
            string? branchId,
            string? branchToken,
            string? currentCompanyMemberId
            )
        {
            Ip = ip;
            Port = port;
            Username = username;
            Password = password;
            BranchId = branchId;
            BranchToken = branchToken;
            CurrentCompanyMemberId = currentCompanyMemberId;
        }

        public override string ToString()
        {
            return "Credenciales: " +
                $"Ip: {Ip}, " +
                $"Port: {Port}, " +
                $"Username: {Username}, " +
                $"Password: {Password}, " +
                $"BranchId: {BranchId}, " +
                $"BranchToken: {BranchToken}" +
                $"CurrentCompanyMemberId: {CurrentCompanyMemberId}";
        }
    }
}
