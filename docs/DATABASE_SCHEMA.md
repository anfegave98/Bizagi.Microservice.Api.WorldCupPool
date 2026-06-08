# Esquema de Base de Datos — Polla Mundialista

Base de datos relacional PostgreSQL gestionada con Entity Framework Core 8.  
El esquema se genera mediante migraciones EF y se inicializa con el `DatabaseSeeder` al arrancar la API.

---

## Diagrama de relaciones

```
┌──────────┐        ┌──────────────┐        ┌────────────┐
│  Roles   │◄──────►│  UserRoles   │◄──────►│   Users    │
│          │  1:N   │              │  N:1   │            │
└──────────┘        └──────────────┘        └─────┬──────┘
                                                  │ 1:N
                                                  ▼
                                          ┌─────────────────┐
                                          │   Predictions   │
                                          │                 │
                                          └────┬────────────┘
                                               │ N:1
┌──────────┐        ┌──────────────┐      ┌───▼────────────┐
│  Groups  │◄──────►│    Teams     │      │    Matches     │
│          │  1:N   │              │      │                │
└──────────┘        └──────┬───────┘      └───┬────────────┘
                           │                  │ 1:1
                           │ N:1 (HomeTeam)   ▼
                           │ N:1 (AwayTeam)  ┌───────────────┐
                           └────────────────►│  MatchResults │
                                             └───┬───────────┘
                                                 │ 1:N
                                                 ▼
                                          ┌───────────────┐
                                          │   ScoreLogs   │
                                          └───────────────┘
```

---

## Tablas

### Users — Usuarios del sistema

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `INT` | PK, AUTO | Identificador único |
| `UserName` | `NVARCHAR(100)` | NOT NULL, UNIQUE | Nombre de usuario |
| `Email` | `VARCHAR(150)` | NOT NULL, UNIQUE | Correo electrónico |
| `PasswordHash` | `NVARCHAR(500)` | NOT NULL | Hash HMACSHA512 de la contraseña |
| `PasswordSalt` | `NVARCHAR(500)` | NULL | Salt individual para el hash |
| `FullName` | `NVARCHAR(150)` | NOT NULL | Nombre completo |
| `IsActive` | `BIT` | NOT NULL, DEFAULT 1 | Estado de actividad del usuario |
| `LastLoginDate` | `DATETIME2` | NULL | Fecha del último login |
| `State` | `BIT` | NOT NULL, DEFAULT 1 | Eliminación lógica |
| `IdUserCreator` | `INT` | NOT NULL | Identificador del usuario creador |
| `DateCreated` | `DATETIME2` | NOT NULL | Fecha de creación |
| `DateModified` | `DATETIME2` | NULL | Fecha de última modificación |

---

### Roles — Roles de autorización

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `INT` | PK, AUTO | Identificador único |
| `Name` | `VARCHAR(50)` | NOT NULL, UNIQUE | Nombre del rol (`Admin`, `User`) |
| `Description` | `NVARCHAR(200)` | NULL | Descripción del rol |
| `State` | `BIT` | NOT NULL, DEFAULT 1 | Eliminación lógica |
| `DateCreated` | `DATETIME2` | NOT NULL | Fecha de creación |

**Datos iniciales (Seeder):**

| Id | Name | Description |
|---|---|---|
| 1 | `Admin` | Administrador del sistema con acceso total. |
| 2 | `User` | Usuario participante de la Polla Mundialista. |

---

### UserRoles — Relación usuarios / roles

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `INT` | PK, AUTO | Identificador único |
| `IdUser` | `INT` | NOT NULL, FK → Users | Usuario |
| `IdRole` | `INT` | NOT NULL, FK → Roles | Rol asignado |
| `State` | `BIT` | NOT NULL, DEFAULT 1 | Eliminación lógica |
| `DateCreated` | `DATETIME2` | NOT NULL | Fecha de creación |

---

### Groups — Grupos mundialistas

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `INT` | PK, AUTO | Identificador único |
| `Name` | `VARCHAR(20)` | NOT NULL | Nombre del grupo (`Grupo A`, `Grupo B`) |
| `Description` | `NVARCHAR(150)` | NULL | Descripción del grupo |
| `State` | `BIT` | NOT NULL, DEFAULT 1 | Eliminación lógica |
| `DateCreated` | `DATETIME2` | NOT NULL | Fecha de creación |

**Datos iniciales (Seeder):**

| Nombre |
|---|
| Grupo A |
| Grupo B |

---

### Teams — Equipos participantes

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `INT` | PK, AUTO | Identificador único |
| `Name` | `NVARCHAR(100)` | NOT NULL | Nombre del equipo |
| `Code` | `VARCHAR(10)` | NOT NULL | Código corto (`COL`, `BRA`, etc.) |
| `FlagUrl` | `NVARCHAR(300)` | NULL | URL de la bandera |
| `IdGroup` | `INT` | NOT NULL, FK → Groups | Grupo al que pertenece |
| `State` | `BIT` | NOT NULL, DEFAULT 1 | Eliminación lógica |
| `DateCreated` | `DATETIME2` | NOT NULL | Fecha de creación |

**Datos iniciales (Seeder):**

| Equipo | Código | Grupo |
|---|---|---|
| Colombia | COL | Grupo A |
| Brasil | BRA | Grupo A |
| Argentina | ARG | Grupo A |
| Uruguay | URU | Grupo A |
| Francia | FRA | Grupo B |
| Alemania | GER | Grupo B |
| España | ESP | Grupo B |
| Portugal | POR | Grupo B |

---

### Matches — Partidos precargados

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `INT` | PK, AUTO | Identificador único |
| `IdGroup` | `INT` | NOT NULL, FK → Groups | Grupo del partido |
| `IdHomeTeam` | `INT` | NOT NULL, FK → Teams | Equipo local |
| `IdAwayTeam` | `INT` | NOT NULL, FK → Teams | Equipo visitante |
| `MatchDate` | `DATETIME2` | NOT NULL | Fecha y hora del partido |
| `Status` | `VARCHAR(20)` | NOT NULL, DEFAULT `Scheduled` | `Scheduled` / `Finished` |
| `RoundName` | `VARCHAR(50)` | NOT NULL | Nombre de la ronda/jornada |
| `HomeTeamName` | `NVARCHAR(100)` | NOT NULL | Nombre local (desnormalizado) |
| `AwayTeamName` | `NVARCHAR(100)` | NOT NULL | Nombre visitante (desnormalizado) |
| `State` | `BIT` | NOT NULL, DEFAULT 1 | Eliminación lógica |
| `DateCreated` | `DATETIME2` | NOT NULL | Fecha de creación |
| `DateModified` | `DATETIME2` | NULL | Fecha de última modificación |

**Datos iniciales (Seeder) — 12 partidos:**

| # | Grupo | Local | Visitante | Jornada |
|---|---|---|---|---|
| 1 | Grupo A | Colombia | Brasil | Jornada 1 |
| 2 | Grupo A | Argentina | Uruguay | Jornada 1 |
| 3 | Grupo A | Colombia | Argentina | Jornada 2 |
| 4 | Grupo A | Brasil | Uruguay | Jornada 2 |
| 5 | Grupo A | Colombia | Uruguay | Jornada 3 |
| 6 | Grupo A | Brasil | Argentina | Jornada 3 |
| 7 | Grupo B | Francia | Alemania | Jornada 1 |
| 8 | Grupo B | España | Portugal | Jornada 1 |
| 9 | Grupo B | Francia | España | Jornada 2 |
| 10 | Grupo B | Alemania | Portugal | Jornada 2 |
| 11 | Grupo B | Francia | Portugal | Jornada 3 |
| 12 | Grupo B | Alemania | España | Jornada 3 |

---

### Predictions — Predicciones de usuarios

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `INT` | PK, AUTO | Identificador único |
| `IdUser` | `INT` | NOT NULL, FK → Users | Usuario que predijo |
| `IdMatch` | `INT` | NOT NULL, FK → Matches | Partido predicho |
| `HomeGoals` | `INT` | NOT NULL | Goles locales predichos |
| `AwayGoals` | `INT` | NOT NULL | Goles visitantes predichos |
| `Points` | `INT` | NOT NULL, DEFAULT 0 | Puntos calculados |
| `IsCalculated` | `BIT` | NOT NULL, DEFAULT 0 | ¿Puntos ya calculados? |
| `CalculatedDate` | `DATETIME2` | NULL | Fecha del cálculo |
| `State` | `BIT` | NOT NULL, DEFAULT 1 | Eliminación lógica |
| `IdUserCreator` | `INT` | NOT NULL | Usuario creador del registro |
| `DateCreated` | `DATETIME2` | NOT NULL | Fecha de creación |
| `DateModified` | `DATETIME2` | NULL | Fecha de última modificación |

**Restricción única:** `(IdUser, IdMatch)` — un usuario solo puede tener una predicción activa por partido.

---

### MatchResults — Resultados reales

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `INT` | PK, AUTO | Identificador único |
| `IdMatch` | `INT` | NOT NULL, UNIQUE, FK → Matches | Partido finalizado |
| `HomeGoals` | `INT` | NOT NULL | Goles reales locales |
| `AwayGoals` | `INT` | NOT NULL | Goles reales visitantes |
| `RegisteredByUserId` | `INT` | NOT NULL | Admin que registró el resultado |
| `RegisteredDate` | `DATETIME2` | NOT NULL | Fecha de registro |
| `State` | `BIT` | NOT NULL, DEFAULT 1 | Eliminación lógica |
| `DateCreated` | `DATETIME2` | NOT NULL | Fecha de creación |
| `DateModified` | `DATETIME2` | NULL | Fecha de última modificación |

**Restricción única:** `(IdMatch)` — solo puede existir un resultado activo por partido.

---

### ScoreLogs — Trazabilidad del cálculo de puntos

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `Id` | `INT` | PK, AUTO | Identificador único |
| `IdPrediction` | `INT` | NOT NULL, FK → Predictions | Predicción evaluada |
| `IdMatchResult` | `INT` | NOT NULL, FK → MatchResults | Resultado real utilizado |
| `PredictedHomeGoals` | `INT` | NOT NULL | Goles locales predichos |
| `PredictedAwayGoals` | `INT` | NOT NULL | Goles visitantes predichos |
| `RealHomeGoals` | `INT` | NOT NULL | Goles locales reales |
| `RealAwayGoals` | `INT` | NOT NULL | Goles visitantes reales |
| `PointsAssigned` | `INT` | NOT NULL | Puntos asignados |
| `RuleApplied` | `VARCHAR(30)` | NOT NULL | `ExactScore` / `WinnerOrDraw` / `Failed` |
| `CalculationDate` | `DATETIME2` | NOT NULL | Fecha del cálculo |
| `State` | `BIT` | NOT NULL, DEFAULT 1 | Eliminación lógica |

---

## Reglas de negocio reflejadas en el esquema

| Regla | Implementación en BD |
|---|---|
| Un usuario, una predicción por partido | `UNIQUE (IdUser, IdMatch)` en `Predictions` |
| Un resultado real por partido | `UNIQUE (IdMatch)` en `MatchResults` |
| Partido solo puede ser `Scheduled` o `Finished` | `VARCHAR(20)` validado en aplicación |
| Puntos inician en 0 | `DEFAULT 0` en `Predictions.Points` |
| `PasswordHash` y `PasswordSalt` no expuestos | Excluidos de todos los DTOs de respuesta |
| Eliminación lógica en tablas funcionales | Columna `State BIT DEFAULT 1` en todas las tablas |

---

## Mapeo BD → Backend (.NET 8) → Frontend (Angular 18)

| Columna BD | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript |
|---|---|---|---|
| `Id` | `Id` | `Id` | `id` |
| `UserName` | `UserName` | `UserName` | `userName` |
| `Email` | `Email` | `Email` | `email` |
| `PasswordHash` | `PasswordHash` | — (no expuesto) | — |
| `PasswordSalt` | `PasswordSalt` | — (no expuesto) | — |
| `FullName` | `FullName` | `FullName` | `fullName` |
| `IsActive` | `IsActive` | `IsActive` | `isActive` |
| `LastLoginDate` | `LastLoginDate` | `LastLoginDate` | `lastLoginDate` |
| `HomeGoals` | `HomeGoals` | `HomeGoals` | `homeGoals` |
| `AwayGoals` | `AwayGoals` | `AwayGoals` | `awayGoals` |
| `MatchDate` | `MatchDate` | `MatchDate` | `matchDate` |
| `Status` | `Status` | `Status` | `status` |
| `RoundName` | `RoundName` | `RoundName` | `roundName` |
| `HomeTeamName` | `HomeTeamName` | `HomeTeamName` | `homeTeamName` |
| `AwayTeamName` | `AwayTeamName` | `AwayTeamName` | `awayTeamName` |
| `Points` | `Points` | `Points` | `points` |
| `IsCalculated` | `IsCalculated` | `IsCalculated` | `isCalculated` |
| `CalculatedDate` | `CalculatedDate` | `CalculatedDate` | `calculatedDate` |
| `RuleApplied` | `RuleApplied` | `RuleApplied` | `ruleApplied` |
| `PointsAssigned` | `PointsAssigned` | `PointsAssigned` | `pointsAssigned` |
| `RegisteredByUserId` | `RegisteredByUserId` | `RegisteredByUserId` | `registeredByUserId` |
| `RegisteredDate` | `RegisteredDate` | `RegisteredDate` | `registeredDate` |
| `TotalPoints` | — (calculado) | `TotalPoints` | `totalPoints` |
| `PredictionCount` | — (calculado) | `PredictionCount` | `predictionCount` |
| `ExactScoreCount` | — (calculado) | `ExactScoreCount` | `exactScoreCount` |
| `Position` | — (calculado) | `Position` | `position` |

> **Convención:** columnas y propiedades C# en `PascalCase`; propiedades TypeScript en `camelCase`.

---

## Comandos EF Core útiles

```bash
# Crear migración
dotnet ef migrations add <NombreMigración> \
  --project src/Bizagi.Microservice.Api.WorldCupPool.EntityFramework \
  --startup-project src/Bizagi.Microservice.Api.WorldCupPool

# Aplicar migración
dotnet ef database update \
  --project src/Bizagi.Microservice.Api.WorldCupPool.EntityFramework \
  --startup-project src/Bizagi.Microservice.Api.WorldCupPool

# Revertir última migración
dotnet ef migrations remove \
  --project src/Bizagi.Microservice.Api.WorldCupPool.EntityFramework \
  --startup-project src/Bizagi.Microservice.Api.WorldCupPool

# Generar script SQL de la migración
dotnet ef migrations script \
  --project src/Bizagi.Microservice.Api.WorldCupPool.EntityFramework \
  --startup-project src/Bizagi.Microservice.Api.WorldCupPool \
  --output migration.sql
```

---

**Desarrollado por:** Andres Felipe Galeano Velasco  
**Sprint:** 04 – 08 de junio de 2026
