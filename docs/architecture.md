# FinAssist Pro - Technical Architecture Document

## Overview
FinAssist Pro is an AI-powered financial automation platform built on Azure. It leverages microservices, event-driven architecture, and conversational AI to automate financial document handling and improve accessibility.

---

## Service Modules

### 1. Document Uploader
- **Purpose**: Accepts uploads of financial documents.
- **Type**: REST API
- **Trigger**: HTTP (user or system-initiated)
- **Responsibilities**:
  - Authenticate using Azure AD
  - Store uploaded files in Azure Blob Storage
  - Emit `document.uploaded` event to Event Grid
  - Send metadata to Service Bus for Document Processor

### 2. Document Processor
- **Purpose**: Extracts key information from uploaded documents
- **Type**: Azure Function
- **Trigger**: Event Grid or Service Bus
- **Responsibilities**:
  - Retrieve document from Blob Storage
  - Perform metadata extraction
    - Non-AI: Regex/templates
    - AI: Azure Form Recognizer
  - Store results in Cosmos DB
  - Send processing result to Review Orchestrator via Service Bus

### 3. Review Orchestrator
- **Purpose**: Coordinates review for flagged documents
- **Type**: Durable Function or Logic App
- **Trigger**: Service Bus
- **Responsibilities**:
  - Flag low-confidence documents for review
  - Notify reviewers via email or Teams
  - Collect and store review results in Cosmos DB
  - Emit `review.completed` event or call Data Store API

### 4. Data Store API
- **Purpose**: Central API for document data access
- **Type**: REST API
- **Trigger**: HTTP
- **Responsibilities**:
  - Authenticate via Azure AD
  - Read/write document metadata from Cosmos DB or Azure SQL
  - Support queries from chatbot and internal systems

### 5. Chatbot API
- **Purpose**: Interface for chatbot to backend services
- **Type**: REST API + Cognitive Integration
- **Trigger**: HTTP (from Azure Bot Service)
- **Responsibilities**:
  - Process chat messages
  - Use LUIS/OpenAI for intent recognition
  - Call downstream APIs: Data Store, Review Orchestrator, Uploader
  - Provide formatted responses to Bot Framework

### 6. Chatbot UI
- **Purpose**: User interface for conversational access
- **Type**: Web app or Microsoft Teams Bot
- **Trigger**: User interaction
- **Responsibilities**:
  - Authenticate via Azure AD
  - Communicate with Azure Bot Service
  - Display structured bot responses

---

## Shared Services

### Azure Event Grid
- **Used For**: Triggering document processing upon file upload

### Azure Service Bus
- **Used For**: Decoupled messaging between services (e.g., processor to orchestrator)

### Azure API Management
- **Used For**: Centralized API gateway with security and throttling

### Azure Active Directory
- **Used For**: Identity and access management for users and services

---

## Event & Messaging Flows

| Event | Source | Target | Transport |
|-------|--------|--------|-----------|
| `document.uploaded` | Document Uploader | Document Processor | Event Grid |
| `document.processed` | Document Processor | Review Orchestrator | Service Bus |
| `review.completed` | Review Orchestrator | Data Store API | REST/HTTP |
| `chat.query.status` | Chatbot UI | Chatbot API | HTTP/API Gateway |

---

## Storage & Data
- **Raw Documents**: Azure Blob Storage
- **Metadata / Extracted Data**: Azure Cosmos DB or Azure SQL
- **Audit Trail / Logs**: Azure Log Analytics, Application Insights

---

## Security Practices
- Azure AD for AuthN/AuthZ
- Azure Key Vault for secrets
- API throttling via API Management
- RBAC for internal roles

---

## Future Enhancements
- Multilingual chatbot
- Feedback loop for AI model retraining
- Power BI integration for dashboards

