# Plan de Trabajo — Sistema Polla Mundialista

## INFORMACIÓN GENERAL

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

- **Tipo:** Plan de trabajo por historias de usuario
- **Proyecto:** Sistema **Polla Mundialista**
- **Duración:** 1 sprint de 5 días calendario
- **Fecha de inicio:** Jueves 04 de junio de 2026
- **Fecha de fin:** Lunes 8 de junio de 2026
- **Backend:** C# / .NET 8 Microservice Web API
- **Frontend:** Angular 18
- **Base de datos:** Relacional
- **Responsable del desarrollo:** Andres Felipe Galeano Velasco
- **Asignación:** Backend y Frontend
- **Estado:** En planificación

## OBJETIVO DEL DESARROLLO

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Construir una aplicación funcional de **Polla Mundialista** para un grupo privado de usuarios, permitiendo registro/autenticación, consulta de 12 partidos precargados, registro de predicciones, carga de resultados reales por parte del administrador, cálculo automático de puntajes, ranking global y consulta del historial de predicciones por usuario.

El desarrollo debe priorizar una arquitectura clara, mantenible y segura, con separación entre frontend y backend, autenticación mediante **JWT**, control de roles y mecanismos de cifrado/encriptación en frontend y backend para proteger información sensible.

## ALCANCE FUNCIONAL

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

1. **Autenticación y usuarios**
   - Registro de usuarios.
   - Login de usuarios.
   - Token JWT para autenticación.
   - Roles `User` y `Admin`.

2. **Partidos**
   - Precarga de 12 encuentros de 2 grupos de fase de grupos.
   - Consulta de partidos futuros para predicción.

3. **Predicciones**
   - Registro de goles local y goles visitante.
   - Validación para evitar duplicidad por usuario y partido.
   - Consulta de predicciones propias.

4. **Administración**
   - Vista protegida para rol `Admin`.
   - Carga de resultado final de partido.

5. **Puntuación**
   - 3 puntos por marcador exacto.
   - 1 punto por acierto de ganador o empate.
   - 0 puntos por fallo.

6. **Leaderboard y dashboard**
   - Ranking global ordenado por puntos.
   - Historial de predicciones al seleccionar un usuario.

7. **Seguridad**
   - JWT para autenticación.
   - Autorización por roles.
   - Hash seguro de contraseñas.
   - Encriptación/cifrado de lado frontend cuando aplique.
   - Encriptación/cifrado de lado backend cuando aplique.
   - Configuración de claves, parámetros JWT, CORS, cifrado/encriptación y límites de solicitudes desde `appsettings.json` del backend.
   - Rate limiting configurable para limitar la cantidad de solicitudes por usuario, IP o endpoint según la política definida.
   - CORS restringido.
   - Manejo de errores sin exponer información interna.

8. **Entregables**
   - Repositorio Git público.
   - README.md detallado.
   - Esquema de base de datos.
   - AI_LOG.md con 2 o 3 prompts complejos o bloqueos resueltos con IA.
   - Diagrama de arquitectura o flujo de datos.
   - URL hosteada con usuario y contraseña de prueba para `Admin` y `User`.

## PROYECTOS NUEVOS

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

| Proyecto | Tipo | Framework | Descripción |
|---|---|---|---|
| `Bizagi.Microservice.Api.WorldCupPool` | Backend Microservice | .NET 8 | Microservicio con APIs REST para autenticación, usuarios, partidos, predicciones, resultados, puntajes y leaderboard. |
| `bizagi-worldcup-pool-site` | Frontend  | Angular 18 | Aplicación web para participantes y administradores de la Polla Mundialista. |
| Base de datos relacional | Persistencia | SQL relacional | Modelo de usuarios, roles, partidos, predicciones, resultados y puntuaciones. |

## ESTRUCTURA PROPUESTA DEL BACKEND (.NET 8)

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

```text
Bizagi.Microservice.Api.WorldCupPool/
├── src/
│   ├── Bizagi.Microservice.Api.WorldCupPool/                         # API Host: Controllers, Program.cs, appsettings
│   ├── Bizagi.Microservice.Api.WorldCupPool.Logic/                   # Lógica de negocio y cálculo de puntuación
│   ├── Bizagi.Microservice.Api.WorldCupPool.Abstractions/            # Interfaces de repositorios, lógica y servicios
│   ├── Bizagi.Microservice.Api.WorldCupPool.EntityFramework/         # DbContext, configuraciones EF y repositorios
│   ├── Bizagi.Microservice.Api.WorldCupPool.Entities/                # Entidades de dominio
│   └── Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object/    # DTOs, filtros, responses y enums

```

## ESTRUCTURA PROPUESTA DEL FRONTEND (Angular 18)

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

```text
bizagi-worldcup-pool-site/
├── src/
│   ├── app/
│   │   ├── auth/
│   │   │   ├── components/
│   │   │   ├── guards/
│   │   │   ├── interceptors/
│   │   │   └── services/
│   │   ├── pool/
│   │   │   ├── components/
│   │   │   │   ├── match-list/
│   │   │   │   ├── prediction-form/
│   │   │   │   ├── admin-results/
│   │   │   │   ├── leaderboard/
│   │   │   │   └── user-history/
│   │   │   ├── models/
│   │   │   ├── services/
│   │   │   └── facades/
│   │   └── shared/
│   └── environments/
└── README.md
```

## MODELO DE DATOS PROPUESTO

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

| Tabla | Propósito |
|---|---|
| `Users` | Usuarios registrados del sistema. |
| `Roles` | Roles disponibles: `Admin` y `User`. |
| `UserRoles` | Relación entre usuarios y roles. |
| `Groups` | Grupos mundialistas usados en la fase de grupos. |
| `Teams` | Equipos participantes. |
| `Matches` | 12 partidos precargados de 2 grupos. |
| `Predictions` | Predicciones realizadas por usuario para cada partido. |
| `MatchResults` | Resultado real registrado por el administrador. |
| `ScoreLogs` | Trazabilidad del cálculo de puntos por predicción. |

## MAPEO BASE DE DATOS → BACKEND (.NET 8) → FRONTEND (Angular 18)

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

**Regla general de nomenclatura:** las columnas de base de datos y las propiedades C# se definen en PascalCase; las propiedades TypeScript se definen en camelCase. Los nombres de clases, DTOs, entidades, servicios y endpoints deben mantenerse alineados entre backend y frontend para evitar inconsistencias de contrato.

### Tabla: Users — Usuarios del sistema

| Columna BD | Tipo SQL | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript | HU |
|---|---|---|---|---|---|
| Id | INT | Id | Id | id | HU-002 |
| UserName | NVARCHAR(100) | UserName | UserName | userName | HU-002 |
| Email | VARCHAR(150) | Email | Email | email | HU-002 |
| PasswordHash | NVARCHAR(500) | PasswordHash | — | — | HU-002, HU-008 |
| PasswordSalt | NVARCHAR(500) NULL | PasswordSalt | — | — | HU-002, HU-008 |
| FullName | NVARCHAR(150) | FullName | FullName | fullName | HU-002 |
| IsActive | BIT | IsActive | IsActive | isActive | HU-002 |
| LastLoginDate | DATETIME2 NULL | LastLoginDate | LastLoginDate | lastLoginDate | HU-002 |
| State | BIT | State | State | state | — |
| IdUserCreator | INT | IdUserCreator | IdUserCreator | idUserCreator | — |
| DateCreated | DATETIME2 | DateCreated | DateCreated | dateCreated | — |
| DateModified | DATETIME2 NULL | DateModified | DateModified | dateModified | — |

### Tabla: Roles — Roles de autorización

| Columna BD | Tipo SQL | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript | HU |
|---|---|---|---|---|---|
| Id | INT | Id | Id | id | HU-002 |
| Name | VARCHAR(50) | Name | Name | name | HU-002 |
| Description | NVARCHAR(200) NULL | Description | Description | description | HU-002 |
| State | BIT | State | State | state | — |
| DateCreated | DATETIME2 | DateCreated | DateCreated | dateCreated | — |

### Tabla: UserRoles — Relación usuarios/roles

| Columna BD | Tipo SQL | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript | HU |
|---|---|---|---|---|---|
| Id | INT | Id | Id | id | HU-002 |
| IdUser | INT | IdUser | IdUser | idUser | HU-002 |
| IdRole | INT | IdRole | IdRole | idRole | HU-002 |
| State | BIT | State | State | state | — |
| DateCreated | DATETIME2 | DateCreated | DateCreated | dateCreated | — |

### Tabla: Groups — Grupos mundialistas

| Columna BD | Tipo SQL | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript | HU |
|---|---|---|---|---|---|
| Id | INT | Id | Id | id | HU-003 |
| Name | VARCHAR(20) | Name | Name | name | HU-003 |
| Description | NVARCHAR(150) NULL | Description | Description | description | HU-003 |
| State | BIT | State | State | state | — |
| DateCreated | DATETIME2 | DateCreated | DateCreated | dateCreated | — |

### Tabla: Teams — Equipos participantes

| Columna BD | Tipo SQL | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript | HU |
|---|---|---|---|---|---|
| Id | INT | Id | Id | id | HU-003 |
| Name | NVARCHAR(100) | Name | Name | name | HU-003 |
| Code | VARCHAR(10) | Code | Code | code | HU-003 |
| FlagUrl | NVARCHAR(300) NULL | FlagUrl | FlagUrl | flagUrl | HU-003 |
| IdGroup | INT | IdGroup | IdGroup | idGroup | HU-003 |
| State | BIT | State | State | state | — |
| DateCreated | DATETIME2 | DateCreated | DateCreated | dateCreated | — |

### Tabla: Matches — Partidos precargados

| Columna BD | Tipo SQL | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript | HU |
|---|---|---|---|---|---|
| Id | INT | Id | Id | id | HU-003 |
| IdGroup | INT | IdGroup | IdGroup | idGroup | HU-003 |
| IdHomeTeam | INT | IdHomeTeam | IdHomeTeam | idHomeTeam | HU-003 |
| IdAwayTeam | INT | IdAwayTeam | IdAwayTeam | idAwayTeam | HU-003 |
| MatchDate | DATETIME2 | MatchDate | MatchDate | matchDate | HU-003 |
| Status | VARCHAR(20) | Status | Status | status | HU-003, HU-005 |
| RoundName | VARCHAR(50) | RoundName | RoundName | roundName | HU-003 |
| HomeTeamName | NVARCHAR(100) | HomeTeamName | HomeTeamName | homeTeamName | HU-003 |
| AwayTeamName | NVARCHAR(100) | AwayTeamName | AwayTeamName | awayTeamName | HU-003 |
| State | BIT | State | State | state | — |
| DateCreated | DATETIME2 | DateCreated | DateCreated | dateCreated | — |
| DateModified | DATETIME2 NULL | DateModified | DateModified | dateModified | — |

### Tabla: Predictions — Predicciones de usuarios

| Columna BD | Tipo SQL | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript | HU |
|---|---|---|---|---|---|
| Id | INT | Id | Id | id | HU-004 |
| IdUser | INT | IdUser | IdUser | idUser | HU-004 |
| IdMatch | INT | IdMatch | IdMatch | idMatch | HU-004 |
| HomeGoals | INT | HomeGoals | HomeGoals | homeGoals | HU-004 |
| AwayGoals | INT | AwayGoals | AwayGoals | awayGoals | HU-004 |
| Points | INT | Points | Points | points | HU-006, HU-007 |
| IsCalculated | BIT | IsCalculated | IsCalculated | isCalculated | HU-006 |
| CalculatedDate | DATETIME2 NULL | CalculatedDate | CalculatedDate | calculatedDate | HU-006 |
| State | BIT | State | State | state | — |
| IdUserCreator | INT | IdUserCreator | IdUserCreator | idUserCreator | — |
| DateCreated | DATETIME2 | DateCreated | DateCreated | dateCreated | — |
| DateModified | DATETIME2 NULL | DateModified | DateModified | dateModified | — |

### Tabla: MatchResults — Resultados reales

| Columna BD | Tipo SQL | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript | HU |
|---|---|---|---|---|---|
| Id | INT | Id | Id | id | HU-005 |
| IdMatch | INT | IdMatch | IdMatch | idMatch | HU-005 |
| HomeGoals | INT | HomeGoals | HomeGoals | homeGoals | HU-005 |
| AwayGoals | INT | AwayGoals | AwayGoals | awayGoals | HU-005 |
| RegisteredByUserId | INT | RegisteredByUserId | RegisteredByUserId | registeredByUserId | HU-005 |
| RegisteredDate | DATETIME2 | RegisteredDate | RegisteredDate | registeredDate | HU-005 |
| State | BIT | State | State | state | — |
| DateCreated | DATETIME2 | DateCreated | DateCreated | dateCreated | — |
| DateModified | DATETIME2 NULL | DateModified | DateModified | dateModified | — |

### Tabla: ScoreLogs — Trazabilidad del cálculo de puntaje

| Columna BD | Tipo SQL | Propiedad C# (Entity) | Propiedad C# (DTO) | Propiedad TypeScript | HU |
|---|---|---|---|---|---|
| Id | INT | Id | Id | id | HU-006 |
| IdPrediction | INT | IdPrediction | IdPrediction | idPrediction | HU-006 |
| IdMatchResult | INT | IdMatchResult | IdMatchResult | idMatchResult | HU-006 |
| PredictedHomeGoals | INT | PredictedHomeGoals | PredictedHomeGoals | predictedHomeGoals | HU-006 |
| PredictedAwayGoals | INT | PredictedAwayGoals | PredictedAwayGoals | predictedAwayGoals | HU-006 |
| RealHomeGoals | INT | RealHomeGoals | RealHomeGoals | realHomeGoals | HU-006 |
| RealAwayGoals | INT | RealAwayGoals | RealAwayGoals | realAwayGoals | HU-006 |
| PointsAssigned | INT | PointsAssigned | PointsAssigned | pointsAssigned | HU-006 |
| RuleApplied | VARCHAR(30) | RuleApplied | RuleApplied | ruleApplied | HU-006 |
| CalculationDate | DATETIME2 | CalculationDate | CalculationDate | calculationDate | HU-006 |
| State | BIT | State | State | state | — |

### DTOs principales asociados al mapeo

| DTO Backend | Modelo Frontend | Uso principal | HU |
|---|---|---|---|
| UserDto | UserDto | Información del usuario autenticado | HU-002 |
| LoginRequestDto | LoginRequestDto | Solicitud de autenticación | HU-002 |
| LoginResponseDto | LoginResponseDto | Token JWT y datos mínimos de sesión | HU-002 |
| RegisterUserDto | RegisterUserDto | Registro de usuario | HU-002 |
| MatchDto | MatchDto | Consulta de partidos disponibles | HU-003 |
| PredictionCreateDto | PredictionCreateDto | Registro o actualización de predicción | HU-004 |
| PredictionDto | PredictionDto | Consulta de predicciones del usuario | HU-004, HU-006 |
| MatchResultCreateDto | MatchResultCreateDto | Registro de resultado real por administrador | HU-005 |
| LeaderboardItemDto | LeaderboardItemDto | Fila del ranking global | HU-007 |
| UserPredictionHistoryDto | UserPredictionHistoryDto | Historial de predicciones de un usuario | HU-007 |

### Restricciones funcionales del modelo

- `Predictions` debe tener una restricción única por `IdUser` + `IdMatch` para evitar duplicidad de predicciones por usuario y partido.
- `MatchResults` debe tener una restricción única por `IdMatch` para evitar registrar dos resultados finales activos para el mismo partido.
- `Matches.Status` debe permitir como mínimo los estados `Scheduled` y `Finished`.
- `Predictions.Points` debe iniciar en `0` y actualizarse cuando se registre el resultado real del partido.
- `ScoreLogs.RuleApplied` debe registrar la regla aplicada: `ExactScore`, `WinnerOrDraw`, `Failed`.
- Las tablas funcionales deben incluir `State` para eliminación lógica cuando aplique.
- Los datos sensibles de `Users`, como `PasswordHash` y `PasswordSalt`, no deben exponerse en DTOs ni modelos frontend.

## ROLES DEL SISTEMA

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Los roles del sistema se utilizan tanto en backend como en frontend para controlar el acceso a las funcionalidades de la Polla Mundialista. El backend debe validar autorización mediante JWT y políticas/atributos de rol; el frontend debe ocultar o bloquear vistas y acciones mediante guards e interceptor, sin reemplazar la validación obligatoria del backend.

| Rol | Código Backend | Código Frontend | Permisos Clave | HU |
|---|---|---|---|---|
| Usuario participante | `User` | `User` | Registrarse, iniciar sesión, consultar partidos, registrar/actualizar sus predicciones, consultar sus predicciones, ver leaderboard global y consultar historial de usuarios desde el ranking. | HU-002, HU-003, HU-004, HU-007 |
| Administrador | `Admin` | `Admin` | Todo lo del usuario participante, más cargar resultados reales de partidos, consultar dashboard administrativo, disparar cálculo de puntos y administrar información operativa necesaria para la prueba. | HU-002, HU-005, HU-006, HU-007 |
| Público / Anónimo | `Anonymous` | `Anonymous` | Acceder únicamente a endpoints públicos como registro, login y health check. Aplica rate limiting por IP en endpoints públicos. | HU-002, HU-008 |

### Reglas de autorización por rol

- `User` puede crear o actualizar únicamente sus propias predicciones.
- `User` no puede cargar resultados reales ni acceder al dashboard administrativo.
- `Admin` puede cargar resultados reales y consultar indicadores administrativos.
- `Admin` puede visualizar el ranking y el historial de predicciones igual que un usuario participante.
- Los endpoints protegidos deben validar token JWT válido, rol y usuario autenticado.
- Los endpoints públicos deben aplicar rate limiting por IP para reducir abuso de solicitudes.
- El frontend debe implementar `AuthGuard` y `RoleGuard`, pero la autorización definitiva siempre debe quedar en backend.

## CONFIGURACIONES BACKEND — APPSETTINGS.JSON

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Las configuraciones técnicas que puedan variar entre ambientes deben quedar parametrizadas desde el `appsettings.json` del backend o sus equivalentes por ambiente (`appsettings.Development.json`, `appsettings.Production.json`, variables de entorno o secretos del hosting). No deben quedar valores sensibles quemados directamente en código fuente.

### Configuraciones requeridas

- **JWT:** issuer, audience, secret/key, tiempo de expiración y configuración de renovación si aplica.
- **Encryption:** llave de cifrado, vector/IV si aplica, algoritmo y bandera para habilitar o deshabilitar cifrado de datos sensibles según ambiente.
- **RateLimiting:** límite de solicitudes, ventana de tiempo, política por usuario autenticado, política por IP para endpoints públicos y código de respuesta cuando se exceda el límite.
- **Cors:** orígenes permitidos del frontend y métodos/headers habilitados.
- **Database:** cadena de conexión a la base de datos relacional.
- **Swagger:** bandera para habilitar Swagger únicamente en desarrollo o ambientes autorizados.
- **SecurityHeaders:** parámetros habilitables para reforzar cabeceras de seguridad cuando aplique.

### Ejemplo de estructura esperada

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WorldCupPool;User Id=***;Password=***;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Issuer": "Bizagi.WorldCupPool",
    "Audience": "bizagi-worldcup-pool-site",
    "SecretKey": "CHANGE_ME_FROM_SECRET_MANAGER_OR_ENVIRONMENT",
    "ExpirationMinutes": 60
  },
  "Encryption": {
    "Enabled": true,
    "Algorithm": "AES",
    "Key": "CHANGE_ME_FROM_SECRET_MANAGER_OR_ENVIRONMENT",
    "IV": "CHANGE_ME_FROM_SECRET_MANAGER_OR_ENVIRONMENT"
  },
  "RateLimiting": {
    "Enabled": true,
    "PermitLimit": 100,
    "WindowSeconds": 60,
    "QueueLimit": 0,
    "ApplyByAuthenticatedUser": true,
    "ApplyByIpForAnonymous": true,
    "RejectedStatusCode": 429,
    "Policies": {
      "AuthEndpoints": {
        "PermitLimit": 10,
        "WindowSeconds": 60
      },
      "PredictionEndpoints": {
        "PermitLimit": 30,
        "WindowSeconds": 60
      },
      "AdminEndpoints": {
        "PermitLimit": 60,
        "WindowSeconds": 60
      }
    }
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:4200",
      "https://<frontend-host>"
    ]
  },
  "Swagger": {
    "Enabled": true
  }
}
```

### Criterios técnicos de configuración

- El backend debe leer las configuraciones mediante `IConfiguration` u opciones tipadas (`IOptions<T>`).
- La llave de encriptación, el secret JWT y demás valores sensibles no deben quedar versionados con valores reales.
- La configuración de rate limiting debe permitir ajustar límites sin recompilar la aplicación.
- El límite debe aplicar especialmente sobre endpoints de autenticación, predicciones, resultados administrativos y cualquier endpoint expuesto públicamente.
- Cuando se supere el límite permitido, el backend debe responder con HTTP `429 Too Many Requests` y un mensaje controlado.
- En ambientes productivos, Swagger debe permanecer deshabilitado salvo autorización explícita.

## CONTRATOS TÉCNICOS PRINCIPALES — API ENDPOINTS

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

| # | Verbo | Ruta | Descripción | Rol |
|---|---|---|---|---|
| 1 | POST | `api/auth/register` | Registrar usuario | Público |
| 2 | POST | `api/auth/login` | Autenticar usuario y retornar JWT | Público |
| 3 | GET | `api/matches` | Consultar partidos disponibles | User / Admin |
| 4 | POST | `api/predictions` | Registrar o actualizar predicción | User |
| 5 | GET | `api/predictions/me` | Consultar mis predicciones | User |
| 6 | POST | `api/admin/matches/{matchId}/result` | Registrar resultado real | Admin |
| 7 | GET | `api/leaderboard` | Consultar ranking global | User / Admin |
| 8 | GET | `api/leaderboard/{userId}/history` | Consultar historial de un usuario | User / Admin |
| 9 | GET | `api/admin/dashboard` | Consultar indicadores administrativos | Admin |
| 10 | GET | `api/health` | Validar disponibilidad del servicio | Técnico |

## CONTRATOS TÉCNICOS DETALLADOS — API ENDPOINTS

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

**CRÍTICO:** Estos contratos deben ser respetados por backend y frontend. El frontend debe consumir exactamente las rutas, verbos y nombres de propiedades definidos. Cualquier cambio de contrato debe actualizar este documento, los DTOs backend, los modelos TypeScript y los servicios/facades asociados.

### Endpoint 1: Registrar Usuario

| Atributo | Valor |
|---|---|
| Verbo | POST |
| Ruta | `api/auth/register` |
| Controller | `AuthController` |
| Método Backend | `RegisterAsync(RegisterUserDto dto)` |
| Método Frontend Service | `authService.register(dto: RegisterUserDto): Observable<AuthUserDto>` |
| Método Frontend Facade | `authFacade.register(dto: RegisterUserDto): Observable<AuthUserDto>` |
| Auth | Público |
| Rate Limiting | Policy: `AuthEndpoints` |
| HU | HU-002, HU-008 |

**Request Body — RegisterUserDto**

```json
{
  "userName": "string (required, max 100)",
  "fullName": "string (required, max 150)",
  "email": "string (required, max 150, email format)",
  "password": "string (required, min 8)"
}
```

**Response 201 Created — AuthUserDto**

```json
{
  "id": 1,
  "userName": "agaleano",
  "fullName": "Andres Galeano",
  "email": "andres@correo.com",
  "roles": ["User"],
  "isActive": true,
  "dateCreated": "2026-06-04T10:30:00"
}
```

**Validaciones**

- `userName` y `email` deben ser únicos.
- La contraseña debe almacenarse con hash seguro y nunca retornarse en respuesta.
- El rol por defecto para registro público debe ser `User`.
- Si se supera el límite configurado para `AuthEndpoints`, retornar HTTP `429 Too Many Requests`.

### Endpoint 2: Login de Usuario

| Atributo | Valor |
|---|---|
| Verbo | POST |
| Ruta | `api/auth/login` |
| Controller | `AuthController` |
| Método Backend | `LoginAsync(LoginRequestDto dto)` |
| Método Frontend Service | `authService.login(dto: LoginRequestDto): Observable<LoginResponseDto>` |
| Método Frontend Facade | `authFacade.login(dto: LoginRequestDto): Observable<LoginResponseDto>` |
| Auth | Público |
| Rate Limiting | Policy: `AuthEndpoints` |
| HU | HU-002, HU-008 |

**Request Body — LoginRequestDto**

```json
{
  "userNameOrEmail": "string (required)",
  "password": "string (required)"
}
```

**Response 200 OK — LoginResponseDto**

```json
{
  "accessToken": "jwt-token",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "user": {
    "id": 1,
    "userName": "agaleano",
    "fullName": "Andres Galeano",
    "email": "andres@correo.com",
    "roles": ["User"]
  }
}
```

**Validaciones**

- Credenciales inválidas retornan HTTP `401 Unauthorized` con mensaje controlado.
- Usuario inactivo retorna HTTP `403 Forbidden`.
- El JWT debe generarse usando configuración de `appsettings.json`: issuer, audience, secret y expiración.
- El frontend debe almacenar el token de forma controlada y adjuntarlo mediante interceptor.

### Endpoint 3: Consultar Partidos Disponibles

| Atributo | Valor |
|---|---|
| Verbo | GET |
| Ruta | `api/matches` |
| Controller | `MatchesController` |
| Método Backend | `GetAllAsync()` |
| Método Frontend Service | `matchService.getAll(): Observable<MatchDto[]>` |
| Método Frontend Facade | `matchFacade.loadMatches(): Observable<MatchDto[]>` |
| Auth | Requiere rol: `User` o `Admin` |
| Rate Limiting | Policy: `DefaultAuthenticated` |
| HU | HU-003 |

**Response 200 OK — MatchDto[]**

```json
[
  {
    "id": 1,
    "idGroup": 1,
    "groupName": "Grupo A",
    "idHomeTeam": 1,
    "homeTeamName": "Equipo Local",
    "idAwayTeam": 2,
    "awayTeamName": "Equipo Visitante",
    "matchDate": "2026-06-10T15:00:00",
    "roundName": "Fase de grupos",
    "status": "Scheduled"
  }
]
```

**Validaciones**

- Retornar únicamente partidos activos (`State = true`).
- Ordenar por fecha de partido ascendente.
- El estado mínimo esperado es `Scheduled` o `Finished`.

### Endpoint 4: Registrar o Actualizar Predicción

| Atributo | Valor |
|---|---|
| Verbo | POST |
| Ruta | `api/predictions` |
| Controller | `PredictionsController` |
| Método Backend | `CreateOrUpdateAsync(PredictionCreateDto dto)` |
| Método Frontend Service | `predictionService.createOrUpdate(dto: PredictionCreateDto): Observable<PredictionDto>` |
| Método Frontend Facade | `predictionFacade.savePrediction(dto: PredictionCreateDto): Observable<PredictionDto>` |
| Auth | Requiere rol: `User` |
| Rate Limiting | Policy: `PredictionEndpoints` |
| HU | HU-004, HU-008 |

**Request Body — PredictionCreateDto**

```json
{
  "idMatch": 1,
  "homeGoals": 2,
  "awayGoals": 1
}
```

**Response 200 OK — PredictionDto**

```json
{
  "id": 10,
  "idUser": 1,
  "idMatch": 1,
  "homeGoals": 2,
  "awayGoals": 1,
  "points": 0,
  "isCalculated": false,
  "dateCreated": "2026-06-04T12:00:00",
  "dateModified": "2026-06-04T12:10:00"
}
```

**Validaciones**

- `homeGoals` y `awayGoals` deben ser enteros mayores o iguales a cero.
- El usuario autenticado se obtiene desde el JWT; no se recibe `idUser` desde el frontend.
- No se permite registrar o editar predicciones si el partido está en estado `Finished`.
- Debe existir una restricción única por `IdUser` + `IdMatch`.
- Si existe una predicción activa del usuario para el partido, el backend debe actualizarla; si no existe, debe crearla.

### Endpoint 5: Consultar Mis Predicciones

| Atributo | Valor |
|---|---|
| Verbo | GET |
| Ruta | `api/predictions/me` |
| Controller | `PredictionsController` |
| Método Backend | `GetMineAsync()` |
| Método Frontend Service | `predictionService.getMine(): Observable<PredictionDto[]>` |
| Método Frontend Facade | `predictionFacade.loadMyPredictions(): Observable<PredictionDto[]>` |
| Auth | Requiere rol: `User` |
| Rate Limiting | Policy: `DefaultAuthenticated` |
| HU | HU-004, HU-006 |

**Response 200 OK — PredictionDto[]**

```json
[
  {
    "id": 10,
    "idMatch": 1,
    "homeTeamName": "Equipo Local",
    "awayTeamName": "Equipo Visitante",
    "homeGoals": 2,
    "awayGoals": 1,
    "realHomeGoals": null,
    "realAwayGoals": null,
    "points": 0,
    "isCalculated": false,
    "matchStatus": "Scheduled"
  }
]
```

### Endpoint 6: Registrar Resultado Real de Partido

| Atributo | Valor |
|---|---|
| Verbo | POST |
| Ruta | `api/admin/matches/{matchId}/result` |
| Controller | `AdminMatchesController` |
| Método Backend | `RegisterResultAsync(decimal matchId, MatchResultCreateDto dto)` |
| Método Frontend Service | `adminMatchService.registerResult(matchId: number, dto: MatchResultCreateDto): Observable<MatchResultDto>` |
| Método Frontend Facade | `adminMatchFacade.registerResult(matchId: number, dto: MatchResultCreateDto): Observable<MatchResultDto>` |
| Auth | Requiere rol: `Admin` |
| Rate Limiting | Policy: `AdminEndpoints` |
| HU | HU-005, HU-006, HU-008 |

**Request Body — MatchResultCreateDto**

```json
{
  "homeGoals": 3,
  "awayGoals": 1
}
```

**Response 201 Created — MatchResultDto**

```json
{
  "id": 5,
  "idMatch": 1,
  "homeGoals": 3,
  "awayGoals": 1,
  "registeredByUserId": 2,
  "registeredDate": "2026-06-04T14:00:00"
}
```

**Validaciones y flujo interno**

- Solo `Admin` puede registrar resultados reales.
- `homeGoals` y `awayGoals` deben ser enteros mayores o iguales a cero.
- No se debe permitir más de un resultado activo por partido.
- Al registrar el resultado, el partido cambia a estado `Finished`.
- Después de guardar el resultado, se ejecuta el cálculo de puntos para todas las predicciones del partido.
- El cálculo debe registrar trazabilidad en `ScoreLogs`.

### Endpoint 7: Consultar Leaderboard Global

| Atributo | Valor |
|---|---|
| Verbo | GET |
| Ruta | `api/leaderboard` |
| Controller | `LeaderboardController` |
| Método Backend | `GetAsync()` |
| Método Frontend Service | `leaderboardService.get(): Observable<LeaderboardItemDto[]>` |
| Método Frontend Facade | `leaderboardFacade.loadLeaderboard(): Observable<LeaderboardItemDto[]>` |
| Auth | Requiere rol: `User` o `Admin` |
| Rate Limiting | Policy: `DefaultAuthenticated` |
| HU | HU-007 |

**Response 200 OK — LeaderboardItemDto[]**

```json
[
  {
    "position": 1,
    "idUser": 1,
    "userName": "agaleano",
    "fullName": "Andres Galeano",
    "totalPoints": 18,
    "predictionCount": 12,
    "exactScoreCount": 4
  }
]
```

**Validaciones**

- El ranking debe ordenarse por `totalPoints` descendente.
- Solo deben considerarse puntos calculados de partidos finalizados.
- Si hay empate en puntos, se puede ordenar por cantidad de marcadores exactos y luego por nombre de usuario.

### Endpoint 8: Consultar Historial de Predicciones de Usuario

| Atributo | Valor |
|---|---|
| Verbo | GET |
| Ruta | `api/leaderboard/{userId}/history` |
| Controller | `LeaderboardController` |
| Método Backend | `GetUserHistoryAsync(decimal userId)` |
| Método Frontend Service | `leaderboardService.getUserHistory(userId: number): Observable<UserPredictionHistoryDto[]>` |
| Método Frontend Facade | `leaderboardFacade.loadUserHistory(userId: number): Observable<UserPredictionHistoryDto[]>` |
| Auth | Requiere rol: `User` o `Admin` |
| Rate Limiting | Policy: `DefaultAuthenticated` |
| HU | HU-007 |

**Response 200 OK — UserPredictionHistoryDto[]**

```json
[
  {
    "idMatch": 1,
    "homeTeamName": "Equipo Local",
    "awayTeamName": "Equipo Visitante",
    "predictedHomeGoals": 2,
    "predictedAwayGoals": 1,
    "realHomeGoals": 3,
    "realAwayGoals": 1,
    "points": 1,
    "ruleApplied": "WinnerOrDraw",
    "matchStatus": "Finished"
  }
]
```

### Endpoint 9: Consultar Dashboard Administrativo

| Atributo | Valor |
|---|---|
| Verbo | GET |
| Ruta | `api/admin/dashboard` |
| Controller | `AdminDashboardController` |
| Método Backend | `GetDashboardAsync()` |
| Método Frontend Service | `adminDashboardService.getDashboard(): Observable<AdminDashboardDto>` |
| Método Frontend Facade | `adminDashboardFacade.loadDashboard(): Observable<AdminDashboardDto>` |
| Auth | Requiere rol: `Admin` |
| Rate Limiting | Policy: `AdminEndpoints` |
| HU | HU-005, HU-006, HU-007 |

**Response 200 OK — AdminDashboardDto**

```json
{
  "totalUsers": 25,
  "totalMatches": 12,
  "finishedMatches": 6,
  "pendingMatches": 6,
  "totalPredictions": 180,
  "calculatedPredictions": 90
}
```

### Endpoint 10: Health Check

| Atributo | Valor |
|---|---|
| Verbo | GET |
| Ruta | `api/health` |
| Controller | `HealthController` |
| Método Backend | `GetAsync()` |
| Método Frontend Service | No aplica |
| Método Frontend Facade | No aplica |
| Auth | Técnico / Público controlado |
| Rate Limiting | Policy: `PublicEndpoints` |
| HU | HU-001, HU-010 |

**Response 200 OK**

```json
{
  "status": "Healthy",
  "service": "Bizagi.Microservice.Api.WorldCupPool",
  "timestamp": "2026-06-04T14:00:00"
}
```

### Frontend Services — Contrato Consolidado

```typescript
@Injectable({ providedIn: 'root' })
export class AuthService {
  register(dto: RegisterUserDto): Observable<AuthUserDto>;
  login(dto: LoginRequestDto): Observable<LoginResponseDto>;
}

@Injectable({ providedIn: 'root' })
export class MatchService {
  getAll(): Observable<MatchDto[]>;
}

@Injectable({ providedIn: 'root' })
export class PredictionService {
  createOrUpdate(dto: PredictionCreateDto): Observable<PredictionDto>;
  getMine(): Observable<PredictionDto[]>;
}

@Injectable({ providedIn: 'root' })
export class AdminMatchService {
  registerResult(matchId: number, dto: MatchResultCreateDto): Observable<MatchResultDto>;
}

@Injectable({ providedIn: 'root' })
export class LeaderboardService {
  get(): Observable<LeaderboardItemDto[]>;
  getUserHistory(userId: number): Observable<UserPredictionHistoryDto[]>;
}

@Injectable({ providedIn: 'root' })
export class AdminDashboardService {
  getDashboard(): Observable<AdminDashboardDto>;
}
```

### Consideraciones transversales para todos los endpoints

- Todos los endpoints protegidos deben validar JWT y rol en backend.
- Los endpoints públicos deben aplicar rate limiting por IP.
- Los endpoints autenticados deben aplicar rate limiting por usuario autenticado cuando esté habilitado desde `appsettings.json`.
- Los errores deben retornar mensajes controlados sin exponer stack trace ni información sensible.
- La información sensible no debe registrarse en logs.
- Swagger debe habilitarse únicamente si `Swagger:Enabled` está activo y el ambiente lo permite.

## RESUMEN DE HISTORIAS DE USUARIO

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

| # | Historia de Usuario | Backend | Frontend | SP | Prioridad | Día objetivo |
|---|---|---:|---:|---:|---|---|
| HU-001 | Setup técnico, arquitectura base y repositorio | Sí | Sí | 3 | Alta | Día 1 |
| HU-002 | Autenticación, usuarios, JWT, roles y seguridad base | Sí | Sí | 5 | Alta | Día 1-2 |
| HU-003 | Precarga y consulta de partidos | Sí | Sí | 3 | Alta | Día 2 |
| HU-004 | Registro de predicciones | Sí | Sí | 5 | Alta | Día 2-3 |
| HU-005 | Panel administrativo para resultados reales | Sí | Sí | 5 | Alta | Día 3 |
| HU-006 | Cálculo de puntuación y trazabilidad | Sí | Sí | 3 | Alta | Día 3-4 |
| HU-007 | Leaderboard global e historial de usuario | Sí | Sí | 5 | Alta | Día 4 |
| HU-008 | Encriptación, rate limiting y hardening de seguridad | Sí | Sí | 5 | Alta | Día 4 |
| HU-009 | Entregables técnicos, documentación, AI_LOG y arquitectura | Sí | Sí | 2 | Media | Día 5 |
| HU-010 | Pruebas, ajustes, despliegue y entrega final | Sí | Sí | 3 | Alta | Día 5 |
|  | **Total** |  |  | **39 SP** |  |  |

---

# SPRINT 1 — Desarrollo completo Polla Mundialista

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

- **Duración:** 5 días hábiles
- **Inicio:** Jueves 04 de junio de 2026
- **Fin:** Lunes 8 de junio de 2026
- **Responsable:** Andres Felipe Galeano Velasco
- **Sprint Goal:** Construir, documentar, desplegar y entregar una aplicación funcional de Polla Mundialista con autenticación JWT, roles, predicciones, resultados, puntuación, leaderboard, documentación técnica y seguridad aplicada.

## HU-001: Setup técnico, arquitectura base y repositorio

- **Story Points:** 3
- **Prioridad:** Alta
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 1

### User Story

Como desarrollador del sistema, necesito configurar la solución backend, frontend, base de datos y repositorio, para iniciar el desarrollo con una arquitectura clara, separada y mantenible.

### Criterios de aceptación

- [ ] Existe un repositorio Git con carpetas separadas para backend y frontend.
- [ ] Backend creado en .NET 8 con estructura por capas.
- [ ] Frontend creado en Angular 18.
- [ ] Existe conexión configurada a base de datos relacional.
- [ ] Backend y frontend compilan correctamente.

### Subtareas

**Backend**
- Crear solución `Bizagi.Microservice.Api.WorldCupPool` en .NET 8.
- Crear proyectos por capas: API, Logic, Abstractions, EntityFramework, Entities y DTO.
- Configurar Entity Framework Core y DbContext.
- Configurar Swagger solo para ambiente de desarrollo.
- Configurar health check básico.
- Crear secciones iniciales de `appsettings.json` para `Jwt`, `Encryption`, `RateLimiting`, `Cors` y `Swagger`.

**Frontend**
- Crear proyecto `bizagi-worldcup-pool-site` en Angular 18.
- Configurar routing principal.
- Crear estructura de carpetas para auth, pool y shared.
- Configurar environments para URL del backend.

## HU-002: Autenticación, usuarios, JWT, roles y seguridad base

- **Story Points:** 5
- **Prioridad:** Alta
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 1-2

### User Story

Como usuario de la Polla Mundialista, necesito registrarme e iniciar sesión de forma segura, para acceder al sistema según mi rol y proteger mis predicciones.

### Criterios de aceptación

 Un usuario puede registrarse con datos básicos y contraseña.
- [ ] Un usuario puede iniciar sesión con credenciales válidas.
- [ ] El backend retorna un token JWT válido.
- [ ] El token JWT incluye usuario y rol.
- [ ] Las contraseñas se almacenan con hash seguro.
- [ ] Existen roles `User` y `Admin`.
- [ ] Las rutas protegidas requieren token JWT.
- [ ] El frontend adjunta automáticamente el JWT mediante interceptor.
- [ ] Las vistas administrativas solo son accesibles para `Admin`.

### Subtareas

**Backend**
- Crear entidades `User`, `Role` y `UserRole`.
- Crear endpoints `api/auth/register` y `api/auth/login`.
- Implementar generación y validación de JWT usando configuración desde `appsettings.json`.
- Implementar hash de contraseña.
- Implementar autorización por roles.
- Crear usuario administrador inicial mediante seeder.

**Frontend**
- Crear formularios de login y registro.
- Crear servicio de autenticación.
- Crear interceptor para adjuntar token JWT.
- Crear guards de autenticación y rol.
- Guardar sesión de forma controlada.

## HU-003: Precarga y consulta de partidos

- **Story Points:** 3
- **Prioridad:** Alta
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 2

### User Story

Como usuario autenticado, necesito consultar los partidos disponibles de la fase de grupos, para registrar mis predicciones antes de que se carguen los resultados reales.

### Criterios de aceptación

- [ ]  El sistema precarga 12 encuentros de 2 grupos mediante seeder inicial.
- [ ] Cada partido contiene grupo, equipo local, equipo visitante, fecha y estado.
- [ ] Los usuarios autenticados pueden consultar la lista de partidos.
- [ ] Los partidos se muestran en frontend de forma clara y ordenada.

### Subtareas

**Backend**
- Crear entidades `Team`, `Group` y `Match`.
- Crear configuraciones EF.
- Crear seeder de 2 grupos y 12 partidos.
- Crear endpoint `GET api/matches`.

**Frontend**
- Crear modelos TypeScript.
- Crear servicio `MatchService`.
- Crear componente `MatchListComponent`.
- Mostrar partidos por grupo.

## HU-004: Registro de predicciones

- **Story Points:** 5
- **Prioridad:** Alta
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 2-3

### User Story

Como usuario participante, necesito registrar mis predicciones de goles por partido, para participar en la Polla Mundialista y competir en el ranking.

### Criterios de aceptación

- [ ] Un usuario puede registrar goles local y goles visitante por partido.
- [ ] Los goles deben ser enteros mayores o iguales a 0.
- [ ] Un usuario solo puede tener una predicción activa por partido.
- [ ] No se permite registrar o editar predicciones para partidos con resultado real cargado.
- [ ] El usuario puede consultar sus predicciones registradas.
- [ ] La predicción se asocia al usuario autenticado desde el JWT.

### Subtareas

**Backend**
- Crear entidad `Prediction`.
- Crear DTOs de creación, actualización y consulta.
- Crear endpoint `POST api/predictions`.
- Crear endpoint `GET api/predictions/me`.
- Validar unicidad por usuario y partido.

**Frontend**
- Crear formulario de predicción por partido.
- Validar goles local y visitante.
- Mostrar predicciones existentes.
- Consumir endpoints de predicciones.

## HU-005: Panel administrativo para resultados reales

- **Story Points:** 5
- **Prioridad:** Alta
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 3

### User Story

Como administrador, necesito registrar el resultado real de cada partido, para cerrar el partido y permitir el cálculo de puntos de las predicciones realizadas.

### Criterios de aceptación

- [ ] Solo `Admin` puede acceder al panel de resultados.
- [ ] El administrador puede ingresar goles reales local y visitante.
- [ ] Los goles reales deben ser enteros mayores o iguales a 0.
- [ ] Al guardar el resultado, el partido cambia a `Finished`.
- [ ] Al registrar resultado real, se dispara el cálculo de puntos.
- [ ] Se registra auditoría funcional de la acción.

### Subtareas

**Backend**
- Crear entidad `MatchResult`.
- Crear endpoint `POST api/admin/matches/{matchId}/result`.
- Validar rol `Admin`.
- Validar existencia del partido.
- Invocar lógica de cálculo de puntos.

**Frontend**
- Crear componente `AdminResultsComponent`.
- Proteger ruta con guard Admin.
- Mostrar partidos pendientes de resultado.
- Crear formulario para cargar resultado final.

## HU-006: Cálculo de puntuación y trazabilidad

- **Story Points:** 3
- **Prioridad:** Alta
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 3-4

### User Story

Como participante, necesito que el sistema calcule automáticamente mis puntos después de registrado el resultado real, para conocer mi desempeño sin cálculos manuales.

### Criterios de aceptación

- [ ] Marcador exacto: 3 puntos.
- [ ] Acierto de ganador o empate: 1 punto.
- [ ] Fallo: 0 puntos.
- [ ] El cálculo se ejecuta al guardar resultado real.
- [ ] La predicción queda marcada con puntos calculados.
- [ ] Se registra trazabilidad del cálculo.

### Subtareas

**Backend**
- Implementar servicio de cálculo de puntuación.
- Crear entidad `ScoreLog`.
- Actualizar predicciones con puntos calculados.

**Frontend**
- Mostrar puntos obtenidos en predicciones.
- Mostrar estado visual de partido finalizado.

## HU-007: Leaderboard global e historial de usuario

- **Story Points:** 5
- **Prioridad:** Alta
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 4

### User Story

Como participante, necesito ver el ranking global y el historial de predicciones de cada usuario, para conocer la posición de los participantes y revisar el detalle de sus aciertos.

### Criterios de aceptación

- [ ] Ranking global ordenado por puntos descendente.
- [ ] El ranking incluye posición, usuario, puntos totales y cantidad de predicciones.
- [ ] Al seleccionar un usuario se muestra su historial.
- [ ] El historial muestra partido, predicción, resultado real y puntos obtenidos.
- [ ] La información es visible para usuarios autenticados.

### Subtareas

**Backend**
- Crear endpoint `GET api/leaderboard`.
- Crear endpoint `GET api/leaderboard/{userId}/history`.
- Crear DTOs de ranking e historial.

**Frontend**
- Crear componente `LeaderboardComponent`.
- Crear vista o modal `UserHistoryComponent`.
- Mostrar tabla ordenada de ranking.

## HU-008: Encriptación, rate limiting y hardening de seguridad

- **Story Points:** 5
- **Prioridad:** Alta
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 4

### User Story

Como usuario del sistema, necesito que mi información sensible esté protegida durante el uso de la aplicación, para reducir riesgos de exposición, manipulación o acceso no autorizado.

### Criterios de aceptación

- Las contraseñas nunca se almacenan ni retornan en texto plano.
- El backend valida todos los endpoints protegidos con JWT.
- El frontend no permite acceder a rutas privadas sin token válido.
- El frontend aplica cifrado/encriptación para datos sensibles antes del envío cuando aplique.
- El backend descifra/valida datos protegidos recibidos cuando aplique.
- El backend cifra información sensible en persistencia cuando aplique.
- Las claves de cifrado/encriptación, secret JWT, expiración del token, orígenes CORS y parámetros de seguridad se leen desde `appsettings.json` o configuración equivalente por ambiente.
- El rate limiting se encuentra habilitado y configurable desde `appsettings.json`.
- El sistema limita la cantidad de solicitudes permitidas por usuario autenticado y por IP en endpoints públicos.
- Los endpoints de login/registro tienen una política de límite más restrictiva que los endpoints internos autenticados.
- Cuando el usuario supera el límite configurado, el backend responde HTTP `429 Too Many Requests` con mensaje controlado.
- CORS se restringe al origen del frontend.
- Swagger no se expone en producción.

### Subtareas

**Backend**
- Configurar JWT Bearer Authentication usando parámetros desde `appsettings.json`.
- Configurar autorización por roles.
- Implementar servicio de cifrado/descifrado usando llave, IV, algoritmo y bandera `Enabled` desde configuración.
- Implementar opciones tipadas para `Jwt`, `Encryption`, `RateLimiting`, `Cors` y `Swagger`.
- Implementar rate limiting configurable por usuario autenticado, por IP para usuarios anónimos y por política de endpoint.
- Aplicar política restrictiva a `api/auth/register` y `api/auth/login`.
- Aplicar política controlada a `api/predictions` para limitar exceso de solicitudes por usuario.
- Retornar HTTP `429 Too Many Requests` cuando se exceda el límite configurado.
- Configurar CORS restringido desde `appsettings.json`.
- Configurar manejo global de errores.
- Evitar registrar en logs llaves, tokens, contraseñas, payloads cifrados o información sensible.

**Frontend**
- Implementar interceptor de autenticación.
- Implementar guards.
- Implementar utilidad de cifrado para datos sensibles cuando aplique, tomando configuración pública no sensible desde environment.
- Mostrar mensaje controlado al usuario cuando el backend responda HTTP `429 Too Many Requests`.
- Evitar almacenamiento innecesario de información sensible.

## HU-009: Entregables técnicos, documentación, AI_LOG y arquitectura

- **Story Points:** 2
- **Prioridad:** Media
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 5

### User Story

Como evaluador técnico, necesito recibir documentación clara del proyecto, arquitectura, uso de IA y pasos de ejecución, para validar la calidad técnica y levantar la solución localmente.

### Criterios de aceptación

- [ ]  Existe `README.md` con pasos de ejecución.
- [ ]  Existe `AI_LOG.md` con 2 o 3 prompts complejos o bloqueos resueltos con IA.
- [ ]  Existe diagrama de arquitectura o flujo de datos.
- [ ]  Existe esquema de base de datos documentado.
- [ ]  El repositorio Git contiene commits representativos.
- [ ]  Se documentan usuarios y contraseñas de prueba.
- [ ]  Se documenta la URL hosteada.

### Subtareas

- Crear README.md.
- Crear AI_LOG.md.
- Crear diagrama C4, componentes o flujo de datos.
- Documentar esquema de base de datos.
- Documentar roles del sistema.
- Documentar contratos técnicos detallados de API endpoints.
- Documentar credenciales de prueba.

## HU-010: Pruebas, ajustes, despliegue y entrega final

- **Story Points:** 3
- **Prioridad:** Alta
- **Asignado a:** Andres Felipe Galeano Velasco
- **Día objetivo:** Día 5

### User Story

Como responsable del desarrollo, necesito probar, ajustar, desplegar y entregar la aplicación, para asegurar que el sistema cumpla el alcance solicitado y pueda ser evaluado funcionalmente.

### Criterios de aceptación

- [ ]  Backend ejecuta sin errores en local.
- [ ]  Frontend ejecuta sin errores en local.
- [ ]  Flujo completo validado: registro, login, predicción, carga de resultado, cálculo de puntos, ranking e historial.
- [ ]  Aplicación hosteada en una URL accesible.
- [ ]  Usuarios Admin y User de prueba creados y documentados.
- [ ]  No existen errores críticos en consola del navegador ni logs backend.

### Subtareas

**Backend**
- Validar endpoints protegidos con JWT.
- Validar seeder de partidos y usuario administrador.
- Preparar configuración para hosting.

**Frontend**
- Validar navegación completa.
- Validar guards e interceptor.
- Generar build productivo.

**Entrega**
- Publicar aplicación.
- Validar URL final.
- Actualizar README con credenciales y URL.
- Realizar commit final.

## MATRIZ DE ASIGNACIÓN DIARIA

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

| Día | Fecha | Backend — Andres Felipe Galeano Velasco | Frontend — Andres Felipe Galeano Velasco | Entregable esperado |
|---|---|---|---|---|
| Día 1 | Jueves 04 de junio de 2026 | Setup .NET 8, capas, entidades base, auth inicial | Setup Angular 18, estructura, login/registro inicial | Proyecto base compilando |
| Día 2 | Viernes 05 de junio de 2026 | JWT, roles, seeder partidos, endpoints matches/predictions | Guards, interceptor, listado partidos, formulario predicción | Login y predicciones funcionales |
| Día 3 | Sabado 06 de junio de 2026 | Resultados reales, cálculo puntos, ScoreLogs | Panel Admin, visualización puntos | Flujo Admin y cálculo operativo |
| Día 4 | Domingo 07 de junio de 2026 | Leaderboard, historial, seguridad/encriptación | Ranking, historial, hardening frontend | Dashboard funcional y protegido |
| Día 5 | Lunes 8 de junio de 2026 | Pruebas, ajustes, documentación técnica | Build, ajustes UI, despliegue | Entrega final hosteada y documentada |

## BURNDOWN ESPERADO

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

| Día | SP completados acumulados | SP restantes | HUs objetivo |
|---|---:|---:|---|
| Día 1 | 3 | 34 | HU-001 |
| Día 2 | 11 | 26 | HU-002, HU-003 |
| Día 3 | 21 | 16 | HU-004, HU-005 |
| Día 4 | 34 | 5 | HU-006, HU-007, HU-008 |
| Día 5 | 39 | 0 | HU-009, HU-010 |

## RIESGOS DEL SPRINT

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

| # | Riesgo | Probabilidad | Impacto | Mitigación |
|---|---|---|---|---|
| R1 | Alcance amplio para un sprint de 5 días con una sola persona en backend y frontend | Alta | Alto | Priorizar flujo funcional completo antes de mejoras visuales. |
| R2 | Tiempo limitado para despliegue y documentación | Media | Alto | Documentar desde el Día 1 y reservar Día 5 para entrega. |
| R3 | Cálculo de puntuación con errores de borde en empates o ganador visitante | Media | Medio | Crear pruebas para victoria local, empate y victoria visitante. |
| R4 | Seguridad incompleta por presión de tiempo | Media | Alto | Implementar JWT, roles, hash de contraseña, guards, rate limiting y configuración segura desde appsettings desde el inicio. |
| R5 | Inconsistencias entre DTOs backend y modelos frontend | Media | Medio | Definir contratos antes de construir componentes. |
| R6 | Problemas de hosting al final del sprint | Media | Alto | Validar estrategia de despliegue antes del Día 5. |
| R7 | Configuración incorrecta de rate limiting puede bloquear usuarios legítimos o dejar endpoints sensibles sin protección | Media | Medio | Definir límites conservadores por ambiente, validar endpoints críticos y documentar parámetros configurables en appsettings. |

## DEFINITION OF DONE

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

- Backend .NET 8 compila y ejecuta correctamente.
- Frontend Angular 18 compila y ejecuta correctamente.
- Base de datos relacional creada con esquema documentado y mapeo BD → Backend → Frontend definido.
- Seeder de 12 partidos funcional.
- Registro y login con JWT funcionales.
- Roles Admin y User aplicados correctamente y documentados en la sección Roles del Sistema.
- Predicciones funcionales por usuario.
- Resultados reales cargados por administrador.
- Cálculo de puntos validado.
- Leaderboard e historial funcionales.
- Seguridad mínima aplicada: JWT, roles, hash de contraseña, guards, interceptor, CORS, rate limiting configurable y cifrado/encriptación donde aplique.
- Contratos técnicos detallados de API endpoints definidos y alineados con backend/frontend.
- README.md completo.
- AI_LOG.md completo.
- Diagrama de arquitectura incluido.
- Repositorio Git público disponible.
- Aplicación hosteada y credenciales de prueba documentadas.

## ENTREGABLES FINALES

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

| Entregable | Descripción | Estado esperado |
|---|---|---|
| URL repositorio Git público | Código fuente backend y frontend | Obligatorio |
| README.md | Instrucciones de instalación, ejecución y despliegue | Obligatorio |
| AI_LOG.md | Evidencia de uso de IA con prompts o bloqueos resueltos | Obligatorio |
| Diagrama de arquitectura | C4, componentes o flujo de datos | Obligatorio |
| Esquema de base de datos | Tablas, relaciones, scripts/seeder y mapeo BD → Backend → Frontend | Obligatorio |
| Contratos técnicos detallados | Roles del sistema, endpoints, request/response, autorización, rate limiting y métodos frontend/backend | Obligatorio |

| URL hosteada | Aplicación publicada para revisión | Obligatorio |
| Usuarios de prueba | Credenciales Admin y User | Obligatorio |



---

**Creado por:** Andres Felipe Galeano Velasco  
**Fecha de creación:** Jueves 04 de junio de 2026  
**Versión del documento:** 1.3  
**Estado:** En planificación
