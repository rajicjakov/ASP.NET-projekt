---
name: semantic-model-generator
description: '**WORKFLOW SKILL** — Analyzes existing codebase to generate semantic database model (semantic-model.md) and routing sitemap (sitemap.md). Reads model classes for entities/properties/relationships and controllers for routes/URLs.'
---

# Semantic Model Generator

## Overview
This skill analyzes the existing ASP.NET MVC or similar codebase to understand the database model and routing structure. It generates two documentation files:

- `semantic-model.md`: Lists all models/classes/tables with main properties and connections between tables
- `sitemap.md`: Lists all available URLs with corresponding controller, action, and view

## Workflow Steps

1. **Discover Models**: Scan the Models/ folder and related files to identify all entity classes
2. **Analyze Relationships**: Examine foreign keys, navigation properties, and annotations to map relationships
3. **Extract Properties**: Document main properties for each model
4. **Discover Routes**: Scan Controllers/ folder to find all route definitions
5. **Map URLs**: Analyze controller actions and views to build URL mappings
6. **Generate Documentation**: Write semantic-model.md and sitemap.md files

## Usage
Invoke this skill when you need to document the database schema and routing structure of an existing project. Useful for:
- Project documentation
- Code reviews
- Understanding legacy codebases
- Generating API documentation

## Output Files
- `semantic-model.md`: Semantic database model documentation
- `sitemap.md`: Routing sitemap documentation

Both files are created/updated in the project root.