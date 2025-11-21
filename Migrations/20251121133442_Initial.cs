using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChameleonFutureAcademyAdminApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cfa_tag",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    description = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfa_tag", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "cfa_user_account",
                columns: table => new
                {
                    course_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    title = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    author = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    thumbnail_url = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfa_user_account", x => x.course_id);
                });

            migrationBuilder.CreateTable(
                name: "cfa_badge",
                columns: table => new
                {
                    badge_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    course_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    title = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    icon_url = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfa_badge", x => x.badge_id);
                    table.ForeignKey(
                        name: "FK_cfa_badge_cfa_user_account_course_id",
                        column: x => x.course_id,
                        principalTable: "cfa_user_account",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cfa_content",
                columns: table => new
                {
                    content_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    course_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    type = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    position = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfa_content", x => x.content_id);
                    table.ForeignKey(
                        name: "FK_cfa_content_cfa_user_account_course_id",
                        column: x => x.course_id,
                        principalTable: "cfa_user_account",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cfa_course_tag",
                columns: table => new
                {
                    course_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    tag_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfa_course_tag", x => new { x.course_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_cfa_course_tag_cfa_tag_tag_id",
                        column: x => x.tag_id,
                        principalTable: "cfa_tag",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cfa_course_tag_cfa_user_account_course_id",
                        column: x => x.course_id,
                        principalTable: "cfa_user_account",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cfa_activity",
                columns: table => new
                {
                    activity_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    content_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    title = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    body = table.Column<string>(type: "CLOB", nullable: false),
                    explanation = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfa_activity", x => x.activity_id);
                    table.ForeignKey(
                        name: "FK_cfa_activity_cfa_content_content_id",
                        column: x => x.content_id,
                        principalTable: "cfa_content",
                        principalColumn: "content_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cfa_lesson",
                columns: table => new
                {
                    lesson_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    content_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    title = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    body = table.Column<string>(type: "CLOB", nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfa_lesson", x => x.lesson_id);
                    table.ForeignKey(
                        name: "FK_cfa_lesson_cfa_content_content_id",
                        column: x => x.content_id,
                        principalTable: "cfa_content",
                        principalColumn: "content_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cfa_activity_option",
                columns: table => new
                {
                    activity_option_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    activity_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    label = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    is_correct = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfa_activity_option", x => x.activity_option_id);
                    table.ForeignKey(
                        name: "FK_cfa_activity_option_cfa_activity_activity_id",
                        column: x => x.activity_id,
                        principalTable: "cfa_activity",
                        principalColumn: "activity_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cfa_activity_content_id",
                table: "cfa_activity",
                column: "content_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cfa_activity_option_activity_id_description",
                table: "cfa_activity_option",
                columns: new[] { "activity_id", "description" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cfa_activity_option_activity_id_label",
                table: "cfa_activity_option",
                columns: new[] { "activity_id", "label" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cfa_badge_course_id",
                table: "cfa_badge",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_cfa_badge_title",
                table: "cfa_badge",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cfa_content_course_id_position",
                table: "cfa_content",
                columns: new[] { "course_id", "position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cfa_course_tag_tag_id",
                table: "cfa_course_tag",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_cfa_lesson_content_id",
                table: "cfa_lesson",
                column: "content_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cfa_tag_description",
                table: "cfa_tag",
                column: "description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cfa_user_account_title",
                table: "cfa_user_account",
                column: "title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cfa_activity_option");

            migrationBuilder.DropTable(
                name: "cfa_badge");

            migrationBuilder.DropTable(
                name: "cfa_course_tag");

            migrationBuilder.DropTable(
                name: "cfa_lesson");

            migrationBuilder.DropTable(
                name: "cfa_activity");

            migrationBuilder.DropTable(
                name: "cfa_tag");

            migrationBuilder.DropTable(
                name: "cfa_content");

            migrationBuilder.DropTable(
                name: "cfa_user_account");
        }
    }
}
