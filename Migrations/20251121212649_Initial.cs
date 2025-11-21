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
                name: "CFA_COURSE",
                columns: table => new
                {
                    COURSE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TITLE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    AUTHOR = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    THUMBNAIL_URL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFA_COURSE", x => x.COURSE_ID);
                });

            migrationBuilder.CreateTable(
                name: "CFA_TAG",
                columns: table => new
                {
                    TAG_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFA_TAG", x => x.TAG_ID);
                });

            migrationBuilder.CreateTable(
                name: "CFA_BADGE",
                columns: table => new
                {
                    BADGE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    COURSE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TITLE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ICON_URL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFA_BADGE", x => x.BADGE_ID);
                    table.ForeignKey(
                        name: "FK_CFA_BADGE_CFA_COURSE_COURSE_ID",
                        column: x => x.COURSE_ID,
                        principalTable: "CFA_COURSE",
                        principalColumn: "COURSE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CFA_CONTENT",
                columns: table => new
                {
                    CONTENT_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    COURSE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TYPE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    POSITION = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFA_CONTENT", x => x.CONTENT_ID);
                    table.ForeignKey(
                        name: "FK_CFA_CONTENT_CFA_COURSE_COURSE_ID",
                        column: x => x.COURSE_ID,
                        principalTable: "CFA_COURSE",
                        principalColumn: "COURSE_ID",
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
                        name: "FK_cfa_course_tag_CFA_COURSE_course_id",
                        column: x => x.course_id,
                        principalTable: "CFA_COURSE",
                        principalColumn: "COURSE_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cfa_course_tag_CFA_TAG_tag_id",
                        column: x => x.tag_id,
                        principalTable: "CFA_TAG",
                        principalColumn: "TAG_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CFA_ACTIVITY",
                columns: table => new
                {
                    ACTIVITY_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CONTENT_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TITLE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    BODY = table.Column<string>(type: "CLOB", nullable: false),
                    EXPLANATION = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFA_ACTIVITY", x => x.ACTIVITY_ID);
                    table.ForeignKey(
                        name: "FK_CFA_ACTIVITY_CFA_CONTENT_CONTENT_ID",
                        column: x => x.CONTENT_ID,
                        principalTable: "CFA_CONTENT",
                        principalColumn: "CONTENT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CFA_LESSON",
                columns: table => new
                {
                    LESSON_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CONTENT_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TITLE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    BODY = table.Column<string>(type: "CLOB", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFA_LESSON", x => x.LESSON_ID);
                    table.ForeignKey(
                        name: "FK_CFA_LESSON_CFA_CONTENT_CONTENT_ID",
                        column: x => x.CONTENT_ID,
                        principalTable: "CFA_CONTENT",
                        principalColumn: "CONTENT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CFA_ACTIVITY_OPTION",
                columns: table => new
                {
                    ACTIVITY_OPTION_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ACTIVITY_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LABEL = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    IS_CORRECT = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFA_ACTIVITY_OPTION", x => x.ACTIVITY_OPTION_ID);
                    table.ForeignKey(
                        name: "FK_CFA_ACTIVITY_OPTION_CFA_ACTIVITY_ACTIVITY_ID",
                        column: x => x.ACTIVITY_ID,
                        principalTable: "CFA_ACTIVITY",
                        principalColumn: "ACTIVITY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CFA_ACTIVITY_CONTENT_ID",
                table: "CFA_ACTIVITY",
                column: "CONTENT_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CFA_ACTIVITY_OPTION_ACTIVITY_ID_DESCRIPTION",
                table: "CFA_ACTIVITY_OPTION",
                columns: new[] { "ACTIVITY_ID", "DESCRIPTION" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CFA_ACTIVITY_OPTION_ACTIVITY_ID_LABEL",
                table: "CFA_ACTIVITY_OPTION",
                columns: new[] { "ACTIVITY_ID", "LABEL" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CFA_BADGE_COURSE_ID",
                table: "CFA_BADGE",
                column: "COURSE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CFA_BADGE_TITLE",
                table: "CFA_BADGE",
                column: "TITLE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CFA_CONTENT_COURSE_ID_POSITION",
                table: "CFA_CONTENT",
                columns: new[] { "COURSE_ID", "POSITION" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CFA_COURSE_TITLE",
                table: "CFA_COURSE",
                column: "TITLE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cfa_course_tag_tag_id",
                table: "cfa_course_tag",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_CFA_LESSON_CONTENT_ID",
                table: "CFA_LESSON",
                column: "CONTENT_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CFA_TAG_DESCRIPTION",
                table: "CFA_TAG",
                column: "DESCRIPTION",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CFA_ACTIVITY_OPTION");

            migrationBuilder.DropTable(
                name: "CFA_BADGE");

            migrationBuilder.DropTable(
                name: "cfa_course_tag");

            migrationBuilder.DropTable(
                name: "CFA_LESSON");

            migrationBuilder.DropTable(
                name: "CFA_ACTIVITY");

            migrationBuilder.DropTable(
                name: "CFA_TAG");

            migrationBuilder.DropTable(
                name: "CFA_CONTENT");

            migrationBuilder.DropTable(
                name: "CFA_COURSE");
        }
    }
}
