﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models
{
    [PrimaryKey("BookSortCallNumber", "BookFormCallNumber")]
    [Index(nameof(BookName))]
    public class Book
    {
        [StringLength(15)]
        [RegularExpression(@"[0-9a-zA-Z-\.\(\)\s]+")]
        [Display(Name = "中图法分类号")]
        [Column(TypeName = "nchar(15)")]
        public string BookSortCallNumber { get; set; }

        [StringLength(15)]
        [RegularExpression(@"[0-9a-zA-Z-\.\(\)\s]+")]
        [Display(Name = "书次号")]
        [Column(TypeName = "nchar(15)")]
        public string BookFormCallNumber { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "书名")]
        [MaxLength(200)]
        public string BookName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "出版社")]
        [MaxLength(100)]
        public string PublishingHouse { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "作者")]
        [MaxLength(100)]
        public string Author { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "出版日期")]
        public DateTime PublicDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "停止发行日期")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "类型")]
        public BookType Type { get; set; }

        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        [Required]
        [RegularExpression(@"\d+")]
        [JsonIgnore]
        public string ManageBy { get; set; }

        [JsonIgnore]
        public List<Location> Locations { get; } = new();
        [JsonIgnore]
        public List<Store> Stores { get; } = new();
    }
    public enum BookType
    {
        图书,
        期刊,
        报纸,
        附书光盘,
        非书资料
    }
}
