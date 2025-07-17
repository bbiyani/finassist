# FinAssist - AI-Driven Financial Automation

FinAssist is a modular, Azure-native platform that automates the processing and management of financial documents. Built as a showcase project, it uses microservices, event-driven architecture, and conversational AI to demonstrate best practices in cloud-native development, TDD/BDD, and secure DevOps.

---

## 🌐 Architecture Overview

- Microservice-based backend with modular responsibilities
- Event-driven communication (Azure Event Grid, Service Bus)
- AI-powered data extraction using Azure Form Recognizer
- Conversational chatbot using Azure Bot Framework + OpenAI
- Centralized API Gateway (Azure API Management)
- Full Azure AD-based security model

---

## 📦 Key Features

- 📄 Document upload and AI extraction pipeline
- 🧠 Flagging and human-in-the-loop review flows
- 💬 Chatbot interface to query document status and initiate tasks
- 📊 Storage and reporting ready (Cosmos DB + Power BI-ready)
- 🔐 Secure, observable, and testable architecture

---

## 🧱 Tech Stack

| Layer              | Technologies Used |
|-------------------|-------------------|
| Cloud & Hosting    | Azure Kubernetes Service / Container Apps |
| Compute            | Azure Functions, Logic Apps |
| Messaging          | Azure Service Bus, Event Grid |
| Storage            | Azure Blob Storage, Cosmos DB, Azure SQL |
| AI / NLP           | Azure Form Recognizer, Azure OpenAI, LUIS |
| API Gateway        | Azure API Management |
| Identity           | Azure Active Directory |
| Bot                | Azure Bot Framework, React Bot UI |

---

## 🚀 Getting Started

> 💡 Azure resources are created manually during the POC. Later, YAML/IaC will be added for automation.

1. Clone the repo:
   ```bash
   git clone https://github.com/bbiyani/finassist.git
   cd finassist
