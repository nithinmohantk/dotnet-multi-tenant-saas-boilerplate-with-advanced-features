# terraform/github/main.tf
terraform {
  required_providers {
    github = {
      source  = "integrations/github"
      version = "~> 5.0"
    }
  }
}

provider "github" {
  token = var.github_token
  owner = var.github_organization
}

resource "github_repository" "main" {
  name        = var.repository_name
  description = var.repository_description
  visibility  = "public" # or "private"

  has_issues      = true
  has_projects    = true
  has_wiki        = false
  has_discussions = true

  allow_merge_commit     = true
  allow_squash_merge     = true
  allow_rebase_merge     = false
  allow_auto_merge       = true
  delete_branch_on_merge = true

  squash_merge_commit_title   = "PR_TITLE"
  squash_merge_commit_message = "PR_BODY"

  vulnerability_alerts = true

  security_and_analysis {
    advanced_security {
      status = "enabled"
    }
    secret_scanning {
      status = "enabled"
    }
    secret_scanning_push_protection {
      status = "enabled"
    }
  }

  topics = [
    "dotnet",
    "azure",
    "kubernetes",
    "terraform",
    "devsecops"
  ]
}

resource "github_branch_default" "main" {
  repository = github_repository.main.name
  branch     = "main"
}

resource "github_repository_ruleset" "main_branch" {
  name        = "Main Branch Protection"
  repository  = github_repository.main.name
  target      = "branch"
  enforcement = "active"

  conditions {
    ref_name {
      include = ["~DEFAULT_BRANCH"]
      exclude = []
    }
  }

  rules {
    pull_request {
      required_approving_review_count   = 2
      dismiss_stale_reviews_on_push     = true
      require_code_owner_review         = true
      require_last_push_approval        = true
      required_review_thread_resolution = true
    }

    required_status_checks {
      strict_required_status_checks_policy = true
      required_check {
        context = "build"
      }
      required_check {
        context = "unit-tests"
      }
      required_check {
        context = "security / sast"
      }
    }

    required_signatures = true
    required_linear_history = true
    deletion = true
    non_fast_forward = true
  }
}

resource "github_actions_repository_permissions" "main" {
  repository = github_repository.main.name
  enabled    = true
  allowed_actions = "selected"
  
  allowed_actions_config {
    github_owned_allowed = true
    verified_allowed     = true
    patterns_allowed     = [
      "docker/*",
      "azure/*",
      "hashicorp/*"
    ]
  }
}
